using SoulsFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace ERParamUtils
{

    class NullProgress : IProgress<float>
    {
        public void Report(float value)
        {
            //throw new NotImplementedException();
        }
    }

    public class SoulsFileUtils
    {


        public static void ExtractDCX(string dcxName, string targetDir) {

            Directory.CreateDirectory(targetDir);

            if (dcxName.Contains("bnd"))
            {

                ProcBnd(dcxName, targetDir);
                return;
            }

            if (dcxName.Contains("emevd")) {

                ProcEMEVD(dcxName,targetDir);
            }
        }

        public static void ProcBnd(string dcxName, string targetDir) {

            var bytes = DCX.Decompress(dcxName, out DCX.Type dcxType);
            string outPath = string.Format(@"{0}/{1}.{2}",
                targetDir,
                Path.GetFileNameWithoutExtension(dcxName),
                 dcxType.ToString());

            File.WriteAllBytes(outPath, bytes);
            BND4Reader bnd = new BND4Reader(dcxName);


            var xws = new XmlWriterSettings();
            xws.Indent = true;
            var xw = XmlWriter.Create(targetDir + @"\" + Path.GetFileNameWithoutExtension(dcxName) + ".xml", xws);
            xw.WriteStartElement("bnd4");
            xw.WriteElementString("filename", dcxName);
            xw.WriteElementString("compression", bnd.Compression.ToString());
            xw.WriteElementString("version", bnd.Version);
            xw.WriteElementString("format", bnd.Format.ToString());
            xw.WriteElementString("bigendian", bnd.BigEndian.ToString());
            xw.WriteElementString("bitbigendian", bnd.BitBigEndian.ToString());
            xw.WriteElementString("unicode", bnd.Unicode.ToString());
            xw.WriteElementString("extended", $"0x{bnd.Extended:X2}");
            xw.WriteElementString("unk04", bnd.Unk04.ToString());
            xw.WriteElementString("unk05", bnd.Unk05.ToString());

            NullProgress testProgress = new();

            string tag = Path.GetFileNameWithoutExtension(dcxName);
            SoulsFileUtils.WriteBinderFiles(bnd, xw, targetDir + "/" + tag, testProgress);

            xw.WriteEndElement();
            xw.Close();
            bnd.Dispose();
        }

        public static void WriteBinderFiles(BinderReader bnd, XmlWriter xw, string targetDir, IProgress<float> progress)
        {
            xw.WriteStartElement("files");
            var pathCounts = new Dictionary<string, int>();
            for (int i = 0; i < bnd.Files.Count; i++)
            {
                BinderFileHeader file = bnd.Files[i];

                string root = "";
                string path;
                if (Binder.HasNames(bnd.Format))
                {
                    path = UnrootBNDPath(file.Name, out root);
                }
                else if (Binder.HasIDs(bnd.Format))
                {
                    path = file.ID.ToString();
                }
                else
                {
                    path = i.ToString();
                }

                xw.WriteStartElement("file");
                xw.WriteElementString("flags", file.Flags.ToString());

                if (Binder.HasIDs(bnd.Format))
                    xw.WriteElementString("id", file.ID.ToString());

                if (root != "")
                    xw.WriteElementString("root", root);

                xw.WriteElementString("path", path);

                string suffix = "";
                if (pathCounts.ContainsKey(path))
                {
                    pathCounts[path]++;
                    suffix = $" ({pathCounts[path]})";
                    xw.WriteElementString("suffix", suffix);
                }
                else
                {
                    pathCounts[path] = 1;
                }

                if (file.CompressionType != DCX.Type.Zlib)
                    xw.WriteElementString("compression_type", file.CompressionType.ToString());

                xw.WriteEndElement();

                byte[] bytes = bnd.ReadFile(file);
                string outPath = $@"{targetDir}\{Path.GetDirectoryName(path)}\{Path.GetFileNameWithoutExtension(path)}{suffix}{Path.GetExtension(path)}";
                Directory.CreateDirectory(Path.GetDirectoryName(outPath));
                File.WriteAllBytes(outPath, bytes);
                progress.Report((float)i / bnd.Files.Count);

                if ( outPath.EndsWith(".fmg"))
                     ProcFMGFile(false, outPath);
                
                //unpack-files-text\m10_00_00_00_low.ivinfobnd\map\m10_00_00_00\tex\Envmap\low\IvInfo\m10_00_00_00_GIIV0000_00.ivInfo
                if (outPath.EndsWith(".ivInfo")) {

                    TryProcFile(outPath);
                }
            }
            xw.WriteEndElement();
        }

        private static string UnrootBNDPath(string name, out string root)
        {
            //"N:\\GR\\data\\INTERROOT_win64\\msg\\zhoCN\\TalkMsg.fmg"
            string[] rootList = { "N:\\GR\\data\\INTERROOT_win64\\" };

            foreach (var p in rootList) {
                if (name.StartsWith(p)) {
                    root = p;
                    name = name.Substring(p.Length);
                    return name;
                }
            }
            root = "";
            return name;
        }

        public static void TryProcFile(string path) {

            //var f = SoulsFile.Read(path);
            //SFUtil.GetDecompressedBR()

            using (FileStream stream = File.OpenRead(path))
            {
                BinaryReaderEx br = new BinaryReaderEx(false, stream);
                //TFormat file = new TFormat();
                br = SFUtil.GetDecompressedBR(br, out DCX.Type compression);

                var xws = new XmlWriterSettings();
                xws.Indent = true;

                var name = path + ".xml";
                var xw = XmlWriter.Create(name, xws);
                xw.WriteStartElement("try");
                xw.WriteElementString("compression", compression.ToString());
                xw.WriteElementString("varintSize", br.VarintSize.ToString());
                xw.WriteElementString("varintLong", br.VarintLong.ToString());
                xw.WriteEndElement();
                xw.Close();


                //file.Compression = compression;
                //file.Read(br);
                //return file;
                stream.Dispose();
            }
            
        }

        //emevd
        static void ProcEMEVDParam(XmlWriter xw, EMEVD.Event ev) {
            xw.WriteStartElement("Parameters");

            for (int i = 0; i < ev.Parameters.Count; i++) {
                xw.WriteStartElement("Param");
                var p = ev.Parameters[i];
                xw.WriteElementString("InstructionIndex", p.InstructionIndex+"");

                xw.WriteEndElement();
            }
            xw.WriteEndElement();
        }
        static void ProcEMEVDInstructions(XmlWriter xw, EMEVD.Event ev)
        {
            xw.WriteStartElement("Instructions");

            for (int i = 0; i < ev.Instructions.Count; i++)
            {
                xw.WriteStartElement("Instruction");
                var instruction = ev.Instructions[i];

                xw.WriteElementString("ID", instruction.ID+"");
                xw.WriteElementString("Bank", instruction.Bank + "");
                xw.WriteElementString("ArgData", instruction.ArgData + "");
                xw.WriteEndElement();
            }
            xw.WriteEndElement();
        }

        public static void ProcEMEVD(string path,string targetDir) {

            string outPath = string.Format(@"{0}/{1}",
                     targetDir,
            Path.GetFileNameWithoutExtension(path));

            EMEVD emevd = EMEVD.Read(path);


            var xws = new XmlWriterSettings();
            xws.Indent = true;

            var name = outPath + ".xml";
            var xw = XmlWriter.Create(name, xws);
            xw.WriteStartElement("events");

            for (int i = 0; i < emevd.Events.Count; i++) {

                var ev = emevd.Events[i];
                xw.WriteStartElement("event");
                xw.WriteElementString("ID", ev.ID+"" );
                xw.WriteElementString("Name", ev.Name);
                ProcEMEVDInstructions(xw, ev);
                xw.WriteEndElement();

            }
            xw.WriteEndElement();
            xw.Close();
        }

        public static void ProcFMGFile(bool bigEndian, string path) {


            FMG fmg = FMG.Read(path);

            var xws = new XmlWriterSettings();
            xws.Indent = true;

            var name = path + ".xml";
            var xw = XmlWriter.Create(name, xws);
            xw.WriteStartElement("msgs");
            
            for (int i = 0; i < fmg.Entries.Count; i++) {

                var entry = fmg.Entries[i];

                if (entry.Text == null)
                    continue;
                xw.WriteStartElement("msg");
                xw.WriteElementString("ID", entry.ID.ToString());
                xw.WriteElementString("Text", entry.Text.ToString());
                xw.WriteEndElement();
                //lines.Add(string.Format("{0};{1}", entry.ID, entry.Text));

            }
            xw.WriteEndElement();
            xw.Close();
            //File.WriteAllLines(path + ".txt",lines);


        }
    }
}

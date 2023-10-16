using SoulsFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ERParamUtils
{

    class NullProgress : IProgress<float>
    {
        public void Report(float value)
        {
            //throw new NotImplementedException();
        }
    }

    public class BinderFileUtils
    {

        public static void ExtractDCX(string dcxName, string targetDir) {

            Directory.CreateDirectory(targetDir);

            var bytes = DCX.Decompress(dcxName, out DCX.Type dcxType);
            string outPath = targetDir + @"/"+ 
                Path.GetFileNameWithoutExtension(dcxName)
                + dcxType.ToString();

            File.WriteAllBytes(outPath, bytes);

            BND4Reader bnd = new BND4Reader(outPath);


            var xws = new XmlWriterSettings();
            xws.Indent = true;
            var xw = XmlWriter.Create( targetDir +@"\"+Path.GetFileNameWithoutExtension(dcxName)+".xml", xws);
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
            BinderFileUtils.WriteBinderFiles(bnd, xw, targetDir+"/"+tag, testProgress);

            xw.WriteEndElement();
            xw.Close();
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

                ProcFMGFile(false, outPath);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils.UpateParam
{
    public class UpdateHelper
    {



        static void LoadIdLines(string fn, List<string> idlines)
        {
            var lines = File.ReadAllLines(fn);
            foreach (var line in lines)
            {

                if (line.StartsWith("#"))
                {
                    continue;
                }
                if (line.Length < 5)
                {
                    continue;
                }
                int pos = line.IndexOf("[");
                if (pos > 0)
                {
                    idlines.Add(line.Substring(0, pos));
                }
                else
                {
                    idlines.Add(line.Trim());
                }
            }
        }

        static void LoadIdLines(string[] lotIdFiles, List<string> idlines)
        {

            foreach (var fn in lotIdFiles)
            {

                var full = GlobalConfig.GetTemplateDir() + "/commons/" + fn;

                LoadIdLines(full, idlines);
            }
        }


        static void DeleteLotAuto(int fileIndex, string namePrefix)
        {
            for (int i = 0; i < 3; i++)
            {
                string fn = GlobalConfig.GetCurrentProject().GetUpdateDir() + "\\lot-a-" + namePrefix + "-" + (fileIndex + i) + ".txt";
                File.Delete(fn);
            }
        }



        static void SaveLotAuto(List<string> lines, int fileIndex, string namePrefix)
        {

            DeleteLotAuto(fileIndex, namePrefix);
            if (lines.Count < 1)
            {
                return;
            }
            string fn = GlobalConfig.GetCurrentProject().GetUpdateDir() + "\\lot-a-" + namePrefix + "-" + fileIndex + ".txt";
            File.WriteAllLines(fn, lines);

        }

        public static void ExecLotAutoId(string equipIdFile, string[] lotIdFiles)
        {

            if (!File.Exists(equipIdFile)) {
                return;
            }
            int equipType = 0;
            string fn = equipIdFile;
            var lines = File.ReadAllLines(fn);
            var updateLines = new List<string>();
            var idLines = new List<string>();
            LoadIdLines(lotIdFiles, idLines);
            int idIndex = 0;
            int fileIndex = 0;
            int allIndex = 0;
            int lotCount = 1;
            string namePrefix = "auto";
            foreach (var line in lines)
            {

                if (line.StartsWith("#"))
                {
                    continue;
                }
                if (line.StartsWith("@@name="))
                {
                    string[] ss = line.Split('=');

                    namePrefix = ss[1];

                    continue;
                }

                if (line.StartsWith("@@et="))
                {
                    string[] ss = line.Split('=');

                    equipType = Int32.Parse(ss[1]);

                    updateLines.Add("");
                    updateLines.Add(string.Format("@@et={0}", equipType));
                    updateLines.Add("@@index=0");
                    continue;
                }

                

                if (line.StartsWith("@@count="))
                {
                    string[] ss = line.Split('=');

                    lotCount = Int32.Parse(ss[1]);

                    updateLines.Add(string.Format("@@count={0}", lotCount));
                    continue;
                }

                if (line.StartsWith("@@getItemFlagId="))
                {
                    updateLines.Add(line);
                    continue;
                }

                if (line.Length < 5)
                {
                    continue;
                }

                string equipId = line.Trim();
                if (line.Contains(";"))
                {
                    string[] ss = line.Split(';');

                    equipId = ss[0].Trim();
                }
                if (equipId.Length < 1)
                    continue;
                if (equipType < 1)
                {
                    continue;
                }

                updateLines.Add("");
                updateLines.Add(string.Format("#{0},{1},{2}", allIndex, idIndex, line));
                string rowId = idLines[idIndex];
                updateLines.Add(string.Format("{0};{1}", rowId, equipId));
                allIndex++;
                idIndex++;
                if (idIndex >= idLines.Count)
                {
                    SaveLotAuto(updateLines, fileIndex, namePrefix);
                    fileIndex++;
                    idIndex = 0;
                    updateLines.Clear();

                    updateLines.Add("");
                    updateLines.Add(string.Format("@@et={0}", equipType));
                    updateLines.Add("@@index=0");
                    updateLines.Add(string.Format("@@count={0}", lotCount));

                }

            }

            if (idIndex > 0)
            {
                SaveLotAuto(updateLines, fileIndex, namePrefix);
            }

            if (allIndex < idLines.Count)
            {
                //
            }
        }
    }
}

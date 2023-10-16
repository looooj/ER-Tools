using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text.Json;

namespace VdfFile
{

    public class VdfLibraryfolders
    {

        Dictionary<string, List<string>> _dict = new();
        JObject _folders;
        public ICollection<string> GetPaths()
        {
            return _dict.Keys;
        }

        public ICollection<string> GetAppIdList(string path)
        {

            return _dict[path];
        }

        void procApps(JObject apps, string path)
        {

            List<string> appIds = new();
            int count = apps.Children().Count();

            var v = apps.First;
            for (int i = 0; i < count; i++)
            {
                appIds.Add(((JProperty)v).Name);
                v = v.Next;
            }

            _dict.Add(path, appIds);
        }

        public void Parse(string vdfFile)
        {

            string jsonText = VdfConvert.FileToJson(vdfFile);
            JObject jobj = JObject.Parse(jsonText);
            _folders =(JObject)jobj["libraryfolders"];

            int count = _folders.Children().Count();
            for (int i = 0; i < count; i++)
            {

                var folder = _folders[i + ""];

                var path = ((JValue)folder["path"]).Value;
                var apps = folder["apps"];
                procApps((JObject)apps, path.ToString());
            }

        }

        public static VdfLibraryfolders Create(string streamPath) {

            string path = streamPath + @"/SteamApps/libraryfolders.vdf";
            VdfLibraryfolders folders = new();
            folders.Parse(path);
            return folders;
        }
    }

    //D:\Program Files (x86)\Steam\steamapps\appmanifest_1245620.acf
    public class VdfAppManifest
    {
        public string AppId { get=>_appId; }
        public string InstallDir { get=>_installDir; }
        public string LastOwner { get=>_lastOwner; }
        private JObject? _appState;
        private string _appId = "";
        private string _installDir = "";
        private string _lastOwner = "";
        public void Parse(string vdfFile)
        {

            string jsonText = VdfConvert.FileToJson(vdfFile);
            JObject jobj = JObject.Parse(jsonText);
            _appState =(JObject)jobj["AppState"];

            _appId = _appState["appid"].ToString();
            _installDir = _appState["installdir"].ToString();
            _lastOwner = _appState["LastOwner"].ToString();
        }

        public static VdfAppManifest Create(string steamPath, string appId) {

            VdfAppManifest appManifest = new();
            string path = steamPath + @"/SteamApps/appmanifest_" + appId + ".acf";
            appManifest.Parse(path);
            return appManifest;
        }
    }

    public class VdfConvert
    {

        static void RemoveTrailingComma(List<string> lines)
        {
            int index = lines.Count - 2;
            while (index > 0)
            {

                if (lines[index] == ",")
                {
                    lines[index] = "";
                    return;
                }
                if (lines[index] == ";"
                    || lines[index] == "{"
                    || lines[index] == "}"
                    )
                    return;
                index--;
            }
        }

        public static string FileToJson(string path)
        {

            string lines = File.ReadAllText(path);

            return TextToJson(lines);
        }

        public static string TextToJson(string lines)
        {

            int sCount = 0;
            int wCount = 0;
            int endFlag = 0;
            List<string> jsonLines = new();
            jsonLines.Add("{");
            for (int i = 0; i < lines.Length; i++)
            {
                char c = lines[i];
                {
                    if (c == '"')
                    {
                        sCount++;
                        if (endFlag == 1)
                        {
                            jsonLines.Add(",");
                        }
                        endFlag = 0;
                    }

                    if (c == '{')
                    {

                    }

                    jsonLines.Add(c + "");

                    if (sCount == 2 && c == '"')
                    {
                        sCount = 0;
                        wCount++;
                        if (wCount == 1)
                            jsonLines.Add(":");
                        if (wCount == 2)
                        {
                            jsonLines.Add(",");
                            wCount = 0;
                        }

                        endFlag = 0;
                    }


                    if (c == '{')
                    {
                        sCount = 0;
                        wCount = 0;
                        endFlag = 0;

                    }


                    if (c == '}')
                    {
                        wCount = 0;
                        sCount = 0;
                        endFlag = 1;
                        RemoveTrailingComma(jsonLines);
                    }

                }

            }

            jsonLines.Add("}");

            string jsonText = string.Join("", jsonLines);
            File.WriteAllText("test.json", jsonText);
            return jsonText;
            //var obj = JsonConvert.DeserializeObject<Dictionary<string,object>>(jsonText);
            //if (obj == null)
            //    obj = new Dictionary<string, object>();

            //return new VdfObject(obj);


        }
    }
}
using SoulsFormats;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils
{
    public class ParamdexConfig
    {
        public GameType Type = GameType.EldenRing;
        

        private static ParamdexConfig instance = new ParamdexConfig();
        static public ParamdexConfig Get() {
            return instance;
        }

        private string GetGameIDForDir()
        {
            switch (Type)
            {
                case GameType.DemonsSouls:
                    return "DES";
                case GameType.DarkSoulsPTDE:
                    return "DS1";
                case GameType.DarkSoulsRemastered:
                    return "DS1R";
                case GameType.DarkSoulsIISOTFS:
                    return "DS2S";
                case GameType.Bloodborne:
                    return "BB";
                case GameType.DarkSoulsIII:
                    return "DS3";
                case GameType.Sekiro:
                    return "SDT";
                case GameType.EldenRing:
                    return "ER";
                default:
                    throw new Exception("Game type not set");
            }
        }

        /*
        public ParamdexConfig(GameType type, string dir)
        {
            this.Type = type;
            this.AssetsDir = dir;
        }
        

        public ParamdexConfig(string dir)
        {
            this.AssetsDir = dir;
        }
        */
        


        public ParamdexConfig() { 
        }

        string AssetsDir
        {
            get => GetAssetsDir();
        }

        public string GetAssetsDir() {
            return GlobalConfig.AssetsDir;
        }

        public string GetAliasAssetsDir()
        {
            return $@"{AssetsDir}\Aliases\{GetGameIDForDir()}";
        }

        public string GetGameOffsetsAssetsDir()
        {
            return $@"{AssetsDir}\GameOffsets\{GetGameIDForDir()}";
        }

        public string GetParamDir()
        {
            return $@"{AssetsDir}\Paramdex\{GetGameIDForDir()}";
        }

        public string GetParamdefDir()
        {
            return $@"{GetParamDir()}\Defs";
        }

        public string GetParamdefPatchDir(ulong patch)
        {
            return $@"{GetParamDir()}\DefsPatch\{patch}";
        }

        public ulong[] GetParamdefPatches()
        {
            var dir = $@"{GetParamDir()}\DefsPatch";
            if (Directory.Exists(dir) )
            {
                var entries = Directory.GetFileSystemEntries(dir);
                return entries.Select(e => ulong.Parse(Path.GetFileNameWithoutExtension(e))).ToArray();
            }
            return new ulong[] { };
        }

        public string GetParamMetaDir()
        {
            return $@"{GetParamDir()}\Meta";
        }

        public string GetParamNamesDir()
        {
            return $@"{GetParamDir()}\Names";
        }

        public PARAMDEF GetParamdefForParam(string paramType)
        {
            PARAMDEF pd = PARAMDEF.XmlDeserialize($@"{GetParamdefDir()}\{paramType}.xml");
            return pd;
        }



    }
}

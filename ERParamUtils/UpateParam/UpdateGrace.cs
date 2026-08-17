using ERParamUtils.UpateParam;
using Org.BouncyCastle.Asn1;
using SoulsFormats;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils.UpdateParam
{
    class UpdateGrace
    {

        static string eventflagIdKey = "eventflagId";
        //static string eventflagIdValue = "71190";//"76101";//71801

        static Dictionary<int, string> LoadGraceFile(string fn) {

            Dictionary<int,string> ret = new Dictionary<int,string>();

            var lines = File.ReadAllLines(fn);

            foreach (var line in lines)
            {

                var line1 = line.Trim();
                if (line1.StartsWith("#"))
                {
                    continue;
                }
                if (line1.StartsWith("//"))
                {
                    continue;
                }
                if (line1.Length < 6)
                {
                    continue;
                }
                var items = line1.Split(';');

                if (items.Length >= 1)
                {
                    var id = int.Parse(items[0]);
                    if (id < 100000)
                        continue;
                    ret.TryAdd(id, line1);
                }
            }
            return ret;
        }


        static Dictionary<int, string> skipDict= new();
        static Dictionary<int, string> allDict= new();

        public static void UnlockGrace(ParamProject paramProject, UpdateCommand updateCommand)
        {

            int value = updateCommand.GetOption(UpdateParamOptionNames.UnlockGrace);
            UnlockGraceType t = UnlockGraceConfig.ValueToType(value);
            if (t == UnlockGraceType.None)
            {
                return;
            }

            if (value < 1)
                return;


            var skipIdFile = GlobalConfig.TemplateDir + "\\commons\\" + ModConfig.GetUnlockGraceSkipName();
            skipDict = LoadGraceFile(skipIdFile);

            var allIdFile = GlobalConfig.TemplateDir + "\\commons\\" + ModConfig.GetUnlockGraceName();

            allDict = LoadGraceFile(allIdFile);


            var param = paramProject.FindParam(ParamNames.BonfireWarpParam);
            if (param == null)
                return;

            UpdateLogger.Begin(param.Name);


            switch (t)
            {
                case UnlockGraceType.UnlockNormal:
                    UnlockGraceNormal(param, paramProject, updateCommand);
                    break;
                case UnlockGraceType.UnlockCustom:
                    UnlockGraceCustom(param, paramProject, updateCommand);
                    break;
            }
            if (updateCommand.HaveOption(UpdateParamOptionNames.EnableFastTravel))
            {
                if ( ModConfig.GetModType() != ModConfig.ModType.CER )
                    SetMapInfoParam(paramProject, updateCommand);
            }

            //if (updateCommand.HaveOption(UpdateParamOptionNames.UnlockRoundtableHold))
            //{
            //    UnlockRoundtableHold(updateCommand);
            //}
        }

        /*
        static int[] normalSkipRowIds = { };
        static int[] normalSkipRowIds1 = {
                111000,
                100001,100000,
                110000,110001,350000,190000,
                110500,110501,110502,110503,110504,
                150000,150005,160000,160001,160006,
                130001,130002,130000,
                650008,630050,611011,390000,650054,610018,
                640014,640010,640007,
                620047,620062,620043,
                140000,140001,
                120400,120300,120201,120200,120500,120100,
                200000,694005,280000,685003,682002,210001,210100,210007,
                692000,220000,694005,696000,200100,
                341200,341102,341300,341500,341401
               };*/
        private static void UnlockGraceNormal(SoulsParam.Param param, ParamProject paramProject, UpdateCommand updateCommand)
        {

            var index = 0;
            for (int i = 0; i < param.Rows.Count; i++)
            {
                var row = param.Rows[i];

                if (row.ID < 100000)
                {
                    continue;
                }                

                int textId = ParamRowUtils.GetCellInt(row, "textId1", 0);
                if (!allDict.ContainsKey(textId))
                    continue;
                if ( skipDict.ContainsKey(textId))
                    continue;

                //updateCommand.AddItem(row, eventflagIdKey, ModConfig.GetUnlockGraceEventId());
                //updateCommand.AddItem(row, "clearedEventFlagId", 0);
                index++;
                UnlockGraceRow(updateCommand, row, textId,index);
            }
        }


        static string FindUnlockFile(ParamProject paramProject)
        {

            string unlockName = "unlock_grace.txt";
            string fn = paramProject.GetDir() + "\\" + unlockName;
            if (File.Exists(fn))
            {
                return fn;
            } 

            fn = GlobalConfig.TemplateDir + "\\commons\\" + ModConfig.GetUnlockGraceName();
            if (File.Exists(fn))
            {
                return fn;
            }
            return "";
        }

        private static void UnlockGraceCustom(SoulsParam.Param param, ParamProject paramProject, UpdateCommand updateCommand)
        {
            string fn = FindUnlockFile(paramProject);

            UpdateLogger.InfoTime("UnlockGraceCustom [{0}]", fn);

            var customDict = LoadGraceFile(fn);

            var index = 0;
            for (int i = 0; i < param.Rows.Count; i++)
            {

                var row = param.Rows[i];


                int textId = ParamRowUtils.GetCellInt(row, "textId1", 0);
                if (skipDict.ContainsKey(textId))
                    continue;

                if (!customDict.ContainsKey(textId))
                    continue;

                index++;
                UnlockGraceRow(updateCommand, row, textId,index);
            }

        }


        static void UnlockGraceRow(UpdateCommand updateCommand, SoulsParam.Param.Row row, int textId, int index) {

            if (allDict.ContainsKey(textId)) {
                UpdateLogger.InfoParam("[{0}] {1}",index, allDict[textId]);
            }
            
            updateCommand.AddItem(row, eventflagIdKey, ModConfig.GetUnlockGraceEventId());
            updateCommand.AddItem(row, "clearedEventFlagId", 0);

        }

        /*
        private static void UnlockGraceCustom1(SoulsParam.Param param, ParamProject paramProject, UpdateCommand updateCommand)
        {

            string fn = FindUnlockFile(paramProject);

            UpdateLogger.InfoTime("UnlockGraceCustom [{0}]", fn);
            // paramProject.GetDir() + "\\unlock_grace.txt";
            if (fn.Length < 3)
            {
                return;
            }

            var lines = File.ReadAllLines(fn);
            HashSet<int> customIdSet = new();

            foreach (var line in lines)
            {

                var line1 = line.Trim();
                if (line1.StartsWith("#"))
                {
                    continue;
                }
                if (line1.Length < 6)
                {
                    continue;
                }
                var items = line1.Split(';');

                if (items.Length >= 1)
                {
                    var id = int.Parse(items[0]);
                    if (id < 100000)
                        continue;
                    customIdSet.Add(id);
                }
            }
            if (customIdSet.Count < 1)
                return;

            for (int i = 0; i < param.Rows.Count; i++)
            {

                var row = param.Rows[i];


                int textId = ParamRowUtils.GetCellInt(row, "textId1", 0);
                if ( normalSkipRowIds.Contains(textId))
                    continue;

                if (!customIdSet.Contains(textId) )
                    continue;


                updateCommand.AddItem(row, eventflagIdKey, ModConfig.GetUnlockGraceEventId());

                //100000;[Stormveil Castle] Godrick the Grafted;11-clearedEventFlagId; 
                updateCommand.AddItem(row, "clearedEventFlagId", 0);
            }

            //todo
        }
        */

        /*
        public static void UnlockRoundtableHold(UpdateCommand updateCommand)
        {
            if (!ModConfig.UnlockRoundtableHold()) {
                return;
            }

            //111000;Table of Lost Grace;大赐福
            int[] defaultIds = { 111000 };
            foreach (int rowId in defaultIds)
            {
                updateCommand.AddItem(ParamNames.BonfireWarpParam, rowId, eventflagIdKey, eventflagIdValue);
            }
        }
        */

        //MapDefaultInfoParam
        public static void SetMapInfoParam(ParamProject paramProject, UpdateCommand updateCommand)
        {
            var param = paramProject.FindParam(ParamNames.MapDefaultInfoParam);
            if (param == null)
            {
                UpdateLogger.Info("Find MapDefaultInfoParam Fail");
                return;
            }
            string key = "EnableFastTravelEventFlagId";

            for (int i = 0; i < param.Rows.Count; i++)
            {

                var row = param.Rows[i];
                //if (row.Name == null || row.Name.Length < 1)
                //    continue;

                int val = ParamRowUtils.GetCellInt(row, key, -1);
                if (val > 1)
                {
                    //82001
                    updateCommand.AddItem(row, key, "6001");
                }
            }
        }



    }




}

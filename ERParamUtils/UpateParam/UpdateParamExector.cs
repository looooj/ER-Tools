﻿namespace ERParamUtils.UpateParam
{
    public class UpdateParamOptions
    {

        public bool Restore = true;
        public bool Publish = true;

        public List<UpdateParamTask> UpdateTasks = new();

        public void Add(UpdateParamTask task)
        {
            UpdateTasks.Add(task);
        }
    }

    public abstract class UpdateParamTask
    {

        public string Description = "";
        public int OrderNo = 999;
        public string UpdateName = "";

        public abstract void Exec(ParamProject project, UpdateCommand updateCommand);
    };


    public class DefaultShopParamTask : UpdateParamTask
    {
        public DefaultShopParamTask()
        {
            OrderNo = 0;
            Description = "UpdateShopParam: Change Stone Glovewort Price; Change A Visible; Change Sell Amount(Rune Arc,...) ";
        }

        public override void Exec(ParamProject project, UpdateCommand updateCommand)
        {
            UpdateShopLineupParam.ExecDefaultUpdate(project);
        }

    }

    public class DefaultMapLotParamTask : UpdateParamTask
    {
        public DefaultMapLotParamTask()
        {
            OrderNo = 0;
            Description = "UpdateMapLotParam: Change Rune Stone Lot Count 20; Arrow Lot Count 99; ...";
        }

        public override void Exec(ParamProject project, UpdateCommand updateCommand)
        {

            ItemLotParamMap.SetDefaultLot(project, updateCommand);
        }
    }


    public class RemoveRequireTask : UpdateParamTask
    {
        public RemoveRequireTask()
        {
            OrderNo = 0;
            Description = "Remove Weapon, Incantation, Sorcery Require";
        }

        public override void Exec(ParamProject project, UpdateCommand updateCommand)
        {

            UpdateRequire.Exec(project);

        }
    }

    public class BuddyTask : UpdateParamTask
    {

        public BuddyTask()
        {

            OrderNo = 0;
            Description = "Increase summoning area, Remove summon consume HP/MP ";
        }

        public override void Exec(ParamProject project, UpdateCommand updateCommand)
        {

            UpdateBuddyStone.Exec(project);

        }
    }


    public class LotParamTask : UpdateParamTask
    {

        public LotParamTask()
        {

            Description = string.Format("Exec Script Update {0} and {1}",
                ParamNames.ItemLotParamEnemy, ParamNames.ItemLotParamMap);
        }

        public override void Exec(ParamProject project, UpdateCommand updateCommand)
        {
            UpdateItemLot.LoadLotSpecUpdate(ParamNames.ItemLotParamMap,
                UpateFile.UpdateLotMapSpec,
                updateCommand);

            UpateLotBatch.LoadLotBatchUpdate(ParamNames.ItemLotParamMap,
                UpateFile.UpdateLotMapBatch,
               updateCommand);



            UpdateItemLot.LoadLotSpecUpdate(ParamNames.ItemLotParamEnemy,
                UpateFile.UpdateLotEnemySpec,
                updateCommand);

            UpateLotBatch.LoadLotBatchUpdate(ParamNames.ItemLotParamEnemy,
                UpateFile.UpdateLotEnemyBatch,
                updateCommand);
        }


    }

    public class UpdateRowParamTask : UpdateParamTask
    {

        internal string? ParamName { get; set; }
        string? _paramName = null;

        public UpdateRowParamTask()
        {

            Description = "Exec update-row.txt";

            UpdateName = UpateFile.UpdateRowFile;
        }

        public override void Exec(ParamProject project, UpdateCommand updateCommand)
        {
            if (_paramName == null)
            {
                UpdateRow.LoadUpdateRow(UpdateName, updateCommand);
                return;
            }
            else
            {
                UpdateRow.LoadUpdateRow(_paramName, UpdateName, updateCommand);
            }
        }
    }

    public class UpdateItemParamTask : UpdateParamTask
    {

        public UpdateItemParamTask()
        {

            Description = "Exec update-item.txt";

            UpdateName = "update-item.txt";

        }

        public override void Exec(ParamProject project, UpdateCommand updateCommand)
        {

            UpdateCommandItem.LoadUpdateItem(UpdateName, updateCommand);
        }
    }


    public class CharInitParamTask : UpdateRowParamTask
    {
        public CharInitParamTask()
        {
            Description = "Exec char-init.txt Update " + ParamNames.CharaInitParam;
            ParamName = ParamNames.CharaInitParam;
            UpdateName = UpateFile.UpdateCharaInit;
        }


    }


    public class SpEffectParamTask : UpdateRowParamTask
    {

        public SpEffectParamTask()
        {
            Description = "Exec sp-effect.txt Update " + ParamNames.SpEffectParam;
            ParamName = ParamNames.SpEffectParam;
            UpdateName = UpateFile.UpdateSpEffect;
        }

    }

    public class UpdateParamExector
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();


        public static void Exec(ParamProject project, UpdateParamOptions options)
        {

            UpdateLogger.SetDir(project.GetUpdateDir() + @"/logs");

            if (options.Restore)
            {
                project.Restore();
            }

            UpdateCommand updateCommand = new UpdateCommand(project);
            foreach (var task in options.UpdateTasks)
            {
                UpdateLogger.InfoTime("Exec {0}", task.GetType().Name);
                task.Exec(project, updateCommand);
            }


            project.SaveParams();

            if (options.Publish)
            {


                project.Publish();
            }

            UpdateLogger.Save();
        }
    }
}
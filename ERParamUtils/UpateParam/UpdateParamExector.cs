
using SoulsFormats;
using System.Reflection.Metadata.Ecma335;

namespace ERParamUtils.UpdateParam
{
    public class UpdateParamOptions
    {

        public bool Restore = true;
        public bool Publish = true;

        public List<UpdateParamTask> UpdateTasks = new();
        public List<string> UpdateCommandOptions = new();

        public void AddTask(UpdateParamTask task)
        {
            UpdateTasks.Add(task);
        }

        public void AddUpdateCommandOption(string name)
        {

            UpdateCommandOptions.Add(name);
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
            Description = "UpdateShopParam: Change Stone Glovewort Price; Change All Visible; Change Sell Amount(Rune Arc,...) ";
        }

        public override void Exec(ParamProject project, UpdateCommand updateCommand)
        {
            UpdateName = Description;
            UpdateShopLineupParam.ExecDefaultUpdate(project, updateCommand);
        }
    }

    public class SpecShopParamTask : UpdateParamTask
    {
        public SpecShopParamTask()
        {
            OrderNo = 0;
            Description = "Exec shop-spec.txt";
        }

        public override void Exec(ParamProject project, UpdateCommand updateCommand)
        {
            UpdateName = Description;
            UpdateShopLineupParam.ExecSpec(project, updateCommand);
        }
    }

    public class SpecRecipeParamTask : UpdateParamTask
    {

        public SpecRecipeParamTask() {
            OrderNo = 0;
            Description = "Exec recipe-spec.txt";
        }

        public override void Exec(ParamProject project, UpdateCommand updateCommand)
        {
            UpdateShopLineupParamRecipe.ExecSpec(project, updateCommand);
        }
    }

    public class DefaultMapLotParamTask : UpdateParamTask
    {
        public DefaultMapLotParamTask()
        {
            OrderNo = 0;
            Description = "UpdateLotParam: Change Rune Stone Lot Count 20; Arrow Lot Count 99; ...";
        }

        public override void Exec(ParamProject project, UpdateCommand updateCommand)
        {
            UpdateName = Description;
            DefautItemLot.SetDefaultLot(project, updateCommand);
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

            UpdateName = Description;
            ParamUpdateRequire.Exec(project, updateCommand);

        }
    }

    public class RemoveWeightTask : UpdateParamTask
    {
        public RemoveWeightTask()
        {
            OrderNo = 0;
            Description = "Set Weapon, Protector weight = 1";
        }

        public override void Exec(ParamProject project, UpdateCommand updateCommand)
        {

            UpdateName = Description;

            ParamRemoveWeight.Exec(project, updateCommand);

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
            UpdateName = Description;

            UpdateBuddyStone.Exec(project, updateCommand);

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
            UpdateName = UpdateFile.UpdateLotMapSpec;
            UpdateItemLot.LoadLotSpecUpdate(ParamNames.ItemLotParamMap,
                UpdateFile.UpdateLotMapSpec,
                updateCommand);

            UpdateName = UpdateFile.UpdateLotMapBatch;
            UpateLotBatch.LoadLotBatchUpdate(ParamNames.ItemLotParamMap,
                UpdateFile.UpdateLotMapBatch,
               updateCommand);


            UpdateName = UpdateFile.UpdateLotEnemySpec;
            UpdateItemLot.LoadLotSpecUpdate(ParamNames.ItemLotParamEnemy,
                UpdateFile.UpdateLotEnemySpec,
                updateCommand);

            UpdateName = UpdateFile.UpdateLotEnemyBatch;
            UpateLotBatch.LoadLotBatchUpdate(ParamNames.ItemLotParamEnemy,
                UpdateFile.UpdateLotEnemyBatch,
                updateCommand);
        }


    }

    public class UpdateRowParamTask : UpdateParamTask
    {

        internal string? ParamName { get => _paramName; set => _paramName = value; }
        string? _paramName = null;

        public UpdateRowParamTask()
        {

            Description = "Exec update-row.txt";

            UpdateName = UpdateFile.UpdateRowFile;
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
            UpdateName = UpdateFile.UpdateCharaInit;
        }


    }


    public class SpEffectParamTask : UpdateRowParamTask
    {

        public SpEffectParamTask()
        {
            Description = "Exec sp-effect.txt Update " + ParamNames.SpEffectParam;
            ParamName = ParamNames.SpEffectParam;
            UpdateName = UpdateFile.UpdateSpEffect;
        }

    }

    public class SmithingStoneTask : UpdateParamTask
    {

        public SmithingStoneTask()
        {
            Description = "Update SmithingStone Upgrade 2/4/6->1";

        }

        public override void Exec(ParamProject project, UpdateCommand updateCommand)
        {
            UpdateSmithingStone.Proc(project, updateCommand);
        }
    }


    public class UnlockCraftingTask : UpdateParamTask
    {
        public UnlockCraftingTask()
        {

            Description = "UnlockCrafting";

        }

        public override void Exec(ParamProject paramProject, UpdateCommand updateCommand)
        {
            UpdateShopLineupParamRecipe.UnlockCrafting(paramProject, updateCommand);
        }
    }




    public class UnlockGraceTask : UpdateParamTask
    {
        public UnlockGraceTask()
        {

            Description = "UnlockGrace";

        }

        public override void Exec(ParamProject paramProject, UpdateCommand updateCommand)
        {

            UpdateGrace.UnlockGrace(paramProject, updateCommand);
            UpdateGrace.SetMapInfoParam(paramProject, updateCommand);
        }




    }


    public class ReplaceParamTask : UpdateParamTask
    {


        public ReplaceParamTask() { 

        }

        public override void Exec(ParamProject project, UpdateCommand updateCommand)
        {
            DefautItemLot.SetLotReplace(project, updateCommand);

        }
    }


    public class UpdateParamExector
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();


        public static void Exec(ParamProject paramProject, UpdateParamOptions options)
        {

            UpdateLogger.SetDir(paramProject.GetUpdateDir() + @"/logs");


            UpdateLogger.InfoTime("");
            UpdateLogger.InfoTime("===Begin");


            if (options.Restore)
            {
                UpdateLogger.InfoTime("===Restore");
                paramProject.Restore();
            }

            UpdateShopLineupParamRecipe.Init();

            UpdateCommand updateCommand = new UpdateCommand(paramProject);
            updateCommand.AddOption(options.UpdateCommandOptions);


            updateCommand.AddItem(UpdateCommandItem.Create(ParamNames.PlayerCommonParam, 0, "baseMagicSlotSize", "10"));
            if (updateCommand.HaveOption(UpdateParamOption.ReplaceTalismanPouch))
            {
                updateCommand.AddItem(
                    UpdateCommandItem.Create(ParamNames.PlayerCommonParam, 0, "baseAccSlotNum", "4"));

            }

            if (updateCommand.HaveOption(UpdateParamOption.AddMapPiece)) {
                UpdateShopLineupParamRecipe.AddMapPiece(paramProject, updateCommand);
            }

            //if (updateCommand.HaveOption(UpdateParamOption.AddWhetblade))
            //{
            //    UpdateShopLineupParamRecipe.AddWhetblade(paramProject, updateCommand);
            //}

            UpdateGrace.UnlockGlaceDefault(updateCommand);
            UpdateCharaInit.AddDefault(paramProject,updateCommand);
            options.UpdateTasks.Insert(0, new ReplaceParamTask());

            foreach (var task in options.UpdateTasks)
            {
                try
                {
                    UpdateLogger.InfoTime("Exec {0}", task.GetType().Name);
                    task.Exec(paramProject, updateCommand);
                }
                catch (Exception ex)
                {
                    logger.Error(ex, ex.Message + " " + task.UpdateName);
                    throw new Exception("Exec (" + task.UpdateName + ") Error " + ex.Message);
                }
            }


            updateCommand.Exec(paramProject);
            paramProject.SaveParams();

            if (options.Publish)
            {
                UpdateLogger.InfoTime("===Publish");

                paramProject.Publish();
            }

            UpdateLogger.Save();
            UpdateLogger.InfoTime("===End");
            UpdateLogger.Clear();
        }
    }
}


using ERParamUtils.UpateParam;
using SoulsFormats;
using System.Reflection.Metadata.Ecma335;

namespace ERParamUtils.UpdateParam
{


    public abstract class UpdateParamTask
    {

        public string Description = "";
        public int OrderNo = 999;
        public string UpdateName = "";
        public virtual void ExecBefore(ParamProject project, UpdateCommand updateCommand)
        {
        }
        public abstract void Exec(ParamProject project, UpdateCommand updateCommand);
    };


    public class UpdateShopParamTask : UpdateParamTask
    {
        public UpdateShopParamTask()
        {
            OrderNo = 0;
            Description = "UpdateShopParam: Change Stone Glovewort Price; Change All Visible; Change Sell Amount(Rune Arc,...) ";
        }

        public override void Exec(ParamProject project, UpdateCommand updateCommand)
        {
            UpdateName = Description;
            UpdateShopLineupParam.ExecDefaultUpdate(project, updateCommand);
        }

        public override void ExecBefore(ParamProject project, UpdateCommand updateCommand)
        {
            base.ExecBefore(project, updateCommand);

            updateCommand.SetOption(UpdateParamOptionNames.ReplaceBellBearing,1);

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

    /*
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
    */



    public class ItemLotCountParamTask : UpdateParamTask
    {
        public ItemLotCountParamTask()
        {
            OrderNo = 0;
            Description = "UpdateLotParam: Change Rune Stone Lot Count 20; Arrow Lot Count 99; ...";
        }

        public override void Exec(ParamProject project, UpdateCommand updateCommand)
        {
            UpdateName = Description;
            ItemLotChangeReplace.SetItemLotCount(project, updateCommand);
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

        public override void ExecBefore(ParamProject paramProject, UpdateCommand updateCommand)
        {

            updateCommand.SetOption(UpdateParamOptionNames.ReplaceCookbook, 1);

        }
        public override void Exec(ParamProject paramProject, UpdateCommand updateCommand)
        {
            UpdateShopLineupParamRecipe.UnlockCrafting(paramProject, updateCommand);
        }
    }


    /*

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
    */

    public class ReplaceParamTask : UpdateParamTask
    {


        public ReplaceParamTask()
        {

        }

        public override void Exec(ParamProject project, UpdateCommand updateCommand)
        {
            ItemLotChangeReplace.SetLotReplace(project, updateCommand);

        }
    }



    public class UpdateParamExector
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();


        public static void CreateTaskList(List<UpdateParamTask> updateParamTasks)
        {

            updateParamTasks.Add(new UnlockCraftingTask());
            //updateParamTasks.Add(new SpecRecipeParamTask());

            //updateParamTasks.Add(new UnlockGraceTask());
            updateParamTasks.Add(new UpdateShopParamTask());
            updateParamTasks.Add(new SpecShopParamTask());


            //updateParamTasks.Add(new ItemLotCountParamTask());
            updateParamTasks.Add(new LotParamTask());

            updateParamTasks.Add(new RemoveRequireTask());
            updateParamTasks.Add(new RemoveWeightTask());
            updateParamTasks.Add(new BuddyTask());


            updateParamTasks.Add(new CharInitParamTask());
            updateParamTasks.Add(new SpEffectParamTask());
            updateParamTasks.Add(new UpdateRowParamTask());
            updateParamTasks.Add(new UpdateItemParamTask());

        }

        public static void CreateOptionList(List<UpdateParamOptionNames> updateParamOptions)
        {

            updateParamOptions.Add(new UpdateParamOptionNames(UpdateParamOptionNames.InitMagicSlotAccSlot));
            updateParamOptions.Add(new UpdateParamOptionNames(UpdateParamOptionNames.ReplaceGoldenSeedSacredTear));
            updateParamOptions.Add(new UpdateParamOptionNames(UpdateParamOptionNames.ReplaceScadutreeFragmentSpiritAsh));

            //updateParamOptions.Add(new UpdateParamOption(UpdateParamOption.ReplaceTalismanPouch));
            //updateParamOptions.Add(new UpdateParamOption(UpdateParamOption.ReplaceRune));
            updateParamOptions.Add(new UpdateParamOptionNames(UpdateParamOptionNames.ReplaceStoneswordKey));
            updateParamOptions.Add(new UpdateParamOptionNames(UpdateParamOptionNames.ReplaceRuneArc));
            updateParamOptions.Add(new UpdateParamOptionNames(UpdateParamOptionNames.ReplaceMemoryStone));
            updateParamOptions.Add(new UpdateParamOptionNames(UpdateParamOptionNames.ReplaceFinger));
            updateParamOptions.Add(new UpdateParamOptionNames(UpdateParamOptionNames.ReplaceDeathroot));
            updateParamOptions.Add(new UpdateParamOptionNames(UpdateParamOptionNames.ReplaceDragonHeart));
            updateParamOptions.Add(new UpdateParamOptionNames(UpdateParamOptionNames.ReplaceRemnant));

            //updateParamOptions.Add(new UpdateParamOption(UpdateParamOption.ReplaceGiantCrowSoul));
            //updateParamOptions.Add(new UpdateParamOption(UpdateParamOption.ReplaceLordRune));
            //updateParamOptions.Add(new UpdateParamOption(UpdateParamOption.DoubleGetSoul));

            //updateParamOptions.Add(new UpdateParamOption(UpdateParamOption.ReplaceCookbook));
            updateParamOptions.Add(new UpdateParamOptionNames(UpdateParamOptionNames.AddMapPiece));
            //updateParamOptions.Add(new UpdateParamOption(UpdateParamOption.IncRemnant));
            updateParamOptions.Add(new UpdateParamOptionNames(UpdateParamOptionNames.AddWhetblade));
            //updateParamOptions.Add(new UpdateParamOption(UpdateParamOption.RemoveRemembranceRequire));
        }

        public static void Exec(ParamProject paramProject, UpdateParamExecOptions options)
        {

            UpdateLogger.SetDir(paramProject.GetUpdateDir() + @"/logs");


            UpdateLogger.InfoTime("");
            UpdateLogger.InfoTime("===Begin");


            if (options.Restore)
            {
                UpdateLogger.InfoTime("===Restore");
                paramProject.Restore();
            }

            UpdateShopLineupParamRecipe.Init(paramProject);

            UpdateCommand updateCommand = new UpdateCommand(paramProject);
            updateCommand.AddOption(options.UpdateCommandOptions);


            foreach (var task in options.UpdateTasks)
            {
                try
                {
                    UpdateLogger.InfoTime("ExecBefore {0}", task.GetType().Name);
                    task.ExecBefore(paramProject, updateCommand);
                }
                catch (Exception ex)
                {
                    logger.Error(ex, ex.Message + " " + task.UpdateName);
                    throw new Exception("ExecBefore (" + task.UpdateName + ") Error " + ex.Message);
                }
            }

            //updateCommand.SetOption(UpdateParamOption.ReplaceTalismanPouch, 1);
            UpdateShopLineupParamRecipe.AddBellBearing(paramProject, updateCommand);
            UpdateShopLineupParamRecipe.AddOthers(paramProject, updateCommand);

            if (updateCommand.HaveOption(UpdateParamOptionNames.InitMagicSlotAccSlot))
            {
                updateCommand.AddItem(
                    UpdateCommandItem.Create(ParamNames.PlayerCommonParam, 0, "baseMagicSlotSize", "10"));
                updateCommand.AddItem(
                    UpdateCommandItem.Create(ParamNames.PlayerCommonParam, 0, "baseAccSlotNum", "4"));

                updateCommand.SetOption(UpdateParamOptionNames.ReplaceTalismanPouch, 1);
                updateCommand.SetOption(UpdateParamOptionNames.ReplaceMemoryStone, 1);
            }

            if (updateCommand.HaveOption(UpdateParamOptionNames.AddMapPiece))
            {
                UpdateShopLineupParamRecipe.AddMapPiece(paramProject, updateCommand);
            }

            if (updateCommand.HaveOption(UpdateParamOptionNames.ReplaceGoldenSeedSacredTear))
            {
                UpdateShopLineupParamRecipe.AddSeedTear(paramProject, updateCommand);
            }

            if (updateCommand.HaveOption(UpdateParamOptionNames.ReplaceScadutreeFragmentSpiritAsh))
            {
                UpdateShopLineupParamRecipe.AddFragmentAsh(paramProject, updateCommand);
            }


            if (updateCommand.HaveOption(UpdateParamOptionNames.TimesGetSoul))
            {
                UpdateSoul.Proc(paramProject, updateCommand);
            }

            if (updateCommand.HaveOption(UpdateParamOptionNames.ReplaceGiantCrowSoul))
            {

                //45610068,Bloodbane Giant Crow,11038,0
                updateCommand.AddItem(
                    UpdateCommandItem.Create(ParamNames.NpcParam, 45610068, "getSoul", "10000000"));

            }


            if (updateCommand.HaveOption(UpdateParamOptionNames.AddWhetblade))
            {
                UpdateShopLineupParamRecipe.AddWhetblade(paramProject, updateCommand);
            }

            //for cer mod
            if (updateCommand.HaveOption(UpdateParamOptionNames.ReplaceRemnant)) { 
                UpdateShopLineupParamRecipe.AddRemnant(paramProject, updateCommand);
            }

            UpdateGrace.UnlockGrace(paramProject, updateCommand);

            //UpdateGrace.UnlockGlaceDefault(updateCommand);
            //UpdateCharaInit.AddDefault(paramProject,updateCommand);

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

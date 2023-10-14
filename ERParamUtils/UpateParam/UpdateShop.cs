using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SoulsFormats.MSBE.Event;

namespace ERParamUtils.UpateParam
{
    public class UpdateShopLineupParam
    {

        public static void ExecDefaultUpdate(ParamProject paramProject) {

            UpdateLogger.Begin(ParamNames.ShopLineupParam);

            var param =  paramProject.FindParam(ParamNames.ShopLineupParam);
            if (param == null)
                return;

            foreach (var row in param.Rows) {
                ChangeVisibility(row);


                int equipId = GetEquipId(row);
                if (SpecEquipConfig.IsArrow(equipId) 
                    || SpecEquipConfig.IsPot(equipId)
                    || SpecEquipConfig.IsRemnant(equipId)
                    || SpecEquipConfig.IsSmithingStone(equipId)
                    || SpecEquipConfig.GetSpec(equipId) > 0 
                    )
                {
                    ChangeSellAmount(row, -1);
                }
                ChangeStoneGlovewortPrice(row, equipId);
            }
        }

        static void ChangeStoneGlovewortPrice(FSParam.Param.Row row,int equipId)
        {
            if (SpecEquipConfig.IsSmithingStone(equipId) || SpecEquipConfig.IsGlovewort(equipId))

            {
                string price = "value";
                int value = ParamRowUtils.GetCellInt(row, price,0);
                if (value > 2000)
                {
                    ParamRowUtils.SetCellValue(row, price, 2000);

                    UpdateLogger.InfoRow(row, price, 2000);
                }
            }
        }

        static int GetEquipId(FSParam.Param.Row row)
        {

            return ParamRowUtils.GetCellInt(row, "equipId",0);

        }

        static void ChangeSellAmount(FSParam.Param.Row row, int amount)
        {
            string key = "sellQuantity";
            ParamRowUtils.SetCellValue(row, key, amount);
            UpdateLogger.InfoRow(row, key, amount);

        }

        static void ChangeVisibility(FSParam.Param.Row row)
        {
            if (row.Name == null)
                return;

            if (row.Name.Contains("[") && row.Name.Contains("]"))
            {

                string key = "eventFlag_forRelease";

                if (ParamRowUtils.GetCellInt(row, key, 0) != 0)
                {
                    ParamRowUtils.SetCellValue(row, key, "0");

                    UpdateLogger.InfoRow(row, key, 0);
                }
            }
        }

    }
}

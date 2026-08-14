using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.Library;

namespace CleanCheats
{
    public static class SettlementCommands
    {
        private const float MaxAcceptableValue = 10000f;

        // Token: 0x06000387 RID: 903 RVA: 0x0001930C File Offset: 0x0001750C
        [CommandLineFunctionality.CommandLineArgumentFunction("set_loyalty_of_settlement", "cleancheats")]
        public static string SetLoyaltyOfSettlement(List<string> strings)
        {
            string usage = "Format is \"cleancheats.set_loyalty_of_settlement [SettlementName] | [loyalty]\".";

            if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings))
            {
                return usage;
            }

            List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, false);
            if (separatedNames.Count != 2)
            {
                return usage;
            }

            if (!int.TryParse(separatedNames[1], out int num))
            {
                return "Please enter a positive number\n" + usage;
            }

            if (num > 100 || num < 0)
            {
                return "Loyalty has to be in the range of 0 to 100";
            }

            if (num > MaxAcceptableValue)
            {
                return "The value is too much";
            }

            string settlementName = separatedNames[0];
            if (!CampaignCheats.TryGetObject<Settlement>(settlementName, out Settlement settlement, out string error, null))
            {
                return $"{error}: {settlementName}\n{usage}";
            }

            if (settlement.IsVillage)
            {
                return "Settlement must be castle or town";
            }

            settlement.Town.Loyalty = num;
            return "Success";
        }

        // Token: 0x06000388 RID: 904 RVA: 0x000193FC File Offset: 0x000175FC
        [CommandLineFunctionality.CommandLineArgumentFunction("set_prosperity_of_settlement", "cleancheats")]
        public static string SetProsperityOfSettlement(List<string> strings)
        {
            string usage = "Format is \"cleancheats.set_prosperity_of_settlement [SettlementName/SettlementID] | [Value]\".";

            if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckParameters(strings, 2) || CampaignCheats.CheckHelp(strings))
            {
                return usage;
            }

            List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, false);
            if (separatedNames.Count != 2)
            {
                return usage;
            }

            string settlementName = separatedNames[0];
            if (!CampaignCheats.TryGetObject<Settlement>(settlementName, out Settlement settlement, out string error, null))
            {
                return $"{error}: {settlementName}\n{usage}";
            }

            if (settlement.IsVillage)
            {
                return "Settlement must be castle or town";
            }

            if (!float.TryParse(separatedNames[1], out float num) || num < 0f)
            {
                return "Please enter a positive number\n" + usage;
            }

            if (num > MaxAcceptableValue)
            {
                return "The value is too much";
            }

            settlement.Town.Prosperity = num;
            return "Success";
        }

        // Token: 0x06000389 RID: 905 RVA: 0x000194EC File Offset: 0x000176EC
        [CommandLineFunctionality.CommandLineArgumentFunction("set_militia_of_settlement", "cleancheats")]
        public static string SetMilitiaOfSettlement(List<string> strings)
        {
            string usage = "Format is \"cleancheats.set_militia_of_settlement [SettlementName/SettlementID] | [Value]\".";

            if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckParameters(strings, 2) || CampaignCheats.CheckHelp(strings))
            {
                return usage;
            }

            List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, false);
            if (separatedNames.Count != 2)
            {
                return usage;
            }

            string settlementName = separatedNames[0];
            if (!CampaignCheats.TryGetObject<Settlement>(settlementName, out Settlement settlement, out string error, null))
            {
                return $"{error}: {settlementName}\n{usage}";
            }

            if (!float.TryParse(separatedNames[1], out float num))
            {
                return "Please enter a number\n" + usage;
            }

            if (num > MaxAcceptableValue)
            {
                return "The value is too much";
            }

            settlement.Militia = num;
            return "Success";
        }

        // Token: 0x0600038A RID: 906 RVA: 0x000195C0 File Offset: 0x000177C0
        [CommandLineFunctionality.CommandLineArgumentFunction("set_security_of_settlement", "cleancheats")]
        public static string SetSecurityOfSettlement(List<string> strings)
        {
            string usage = "Format is \"cleancheats.set_security_of_settlement [SettlementName/SettlementID] | [Value]\".";

            if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckParameters(strings, 2) || CampaignCheats.CheckHelp(strings))
            {
                return usage;
            }

            List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, false);
            if (separatedNames.Count != 2)
            {
                return usage;
            }

            string settlementName = separatedNames[0];
            if (!CampaignCheats.TryGetObject<Settlement>(settlementName, out Settlement settlement, out string error, null))
            {
                return $"{error}: {settlementName}\n{usage}";
            }

            if (settlement.IsVillage)
            {
                return "Settlement must be castle or town";
            }

            if (!float.TryParse(separatedNames[1], out float num))
            {
                return "Please enter a number\n" + usage;
            }

            if (num > MaxAcceptableValue)
            {
                return "The value is too much";
            }

            settlement.Town.Security = num;
            return "Success";
        }

        // Token: 0x0600038B RID: 907 RVA: 0x000196A8 File Offset: 0x000178A8
        [CommandLineFunctionality.CommandLineArgumentFunction("set_food_of_settlement", "cleancheats")]
        public static string SetFoodOfSettlement(List<string> strings)
        {
            string usage = "Format is \"cleancheats.set_food_of_settlement [SettlementName/SettlementID] | [Value]\".";

            if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckParameters(strings, 2) || CampaignCheats.CheckHelp(strings))
            {
                return usage;
            }

            List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, false);
            if (separatedNames.Count != 2)
            {
                return usage;
            }

            string settlementName = separatedNames[0];
            if (!CampaignCheats.TryGetObject<Settlement>(settlementName, out Settlement settlement, out string error, null))
            {
                return $"{error}: {settlementName}\n{usage}";
            }

            if (settlement.IsVillage)
            {
                return "Settlement must be castle or town";
            }

            if (!float.TryParse(separatedNames[1], out float num))
            {
                return "Please enter a number\n" + usage;
            }

            if (num > MaxAcceptableValue)
            {
                return "The value is too much";
            }

            settlement.Town.FoodStocks = num;
            return "Success";
        }

        // Token: 0x0600038C RID: 908 RVA: 0x00019790 File Offset: 0x00017990
        [CommandLineFunctionality.CommandLineArgumentFunction("set_hearth_of_settlement", "cleancheats")]
        public static string SetHearthOfSettlement(List<string> strings)
        {
            string usage = "Format is \"cleancheats.set_hearth_of_settlement [SettlementName/SettlementID] | [Value]\".";

            if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckParameters(strings, 2) || CampaignCheats.CheckHelp(strings))
            {
                return usage;
            }

            List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, false);
            if (separatedNames.Count != 2)
            {
                return usage;
            }

            string settlementName = separatedNames[0];

            if (!CampaignCheats.TryGetObject<Settlement>(settlementName, out Settlement settlement, out string error, x => x.IsVillage))
            {
                return $"{error}: {settlementName}\n{usage}";
            }

            if (settlement.Village == null)
            {
                return "Settlement doesn't have hearth variable.";
            }

            if (!float.TryParse(separatedNames[1], out float num))
            {
                return "Please enter a number\n" + usage;
            }

            if (num > MaxAcceptableValue)
            {
                return "The value is too much";
            }

            settlement.Village.Hearth = num;
            return "Success";
        }

        // Token: 0x06000398 RID: 920 RVA: 0x0001A4D0 File Offset: 0x000186D0
        [CommandLineFunctionality.CommandLineArgumentFunction("add_building_level", "cleancheats")]
        public static string AddDevelopment(List<string> strings)
        {
            string usage = "Format is \"cleancheats.add_building_level [SettlementName] | [Building]\".";

            if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings))
            {
                return usage;
            }

            List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, false);
            if (separatedNames.Count != 2)
            {
                return usage;
            }

            if (!CampaignCheats.TryGetObject<Settlement>(separatedNames[0], out Settlement settlement, out string error, null) || !settlement.IsFortification)
            {
                return error + "\n" + usage;
            }

            if (settlement.IsUnderSiege || settlement.IsUnderRaid || settlement.Party.MapEvent != null)
            {
                return "Requested settlement is not suitable right now to take this action";
            }

            string requestedId = separatedNames[1];
            List<Building> settlementBuildings = settlement.Town.Buildings.ToList();

            if (!CampaignCheats.TryGetObject<BuildingType>(requestedId, out BuildingType buildingType, out error,
                    x => settlementBuildings.Any(y => y.BuildingType == x)))
            {
                return error + "\n" + usage;
            }

            Building building = settlementBuildings.First(x => x.BuildingType == buildingType);
            if (building.CurrentLevel < 3)
            {
                building.CurrentLevel = building.CurrentLevel + 1;
                CampaignEventDispatcher.Instance.OnBuildingLevelChanged(settlement.Town, building, 1);
                return $"{building.BuildingType.Name} level increased to {building.CurrentLevel} at {settlement.Name}";
            }

            return $"{building.BuildingType.Name} is already at max level!";
        }
    }
}
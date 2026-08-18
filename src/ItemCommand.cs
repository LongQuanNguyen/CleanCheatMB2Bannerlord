using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace CleanCheats
{
    public static class ItemCommands
    {
        private const float MaxAcceptableValue = 10000f;

        // Token: 0x06000393 RID: 915 RVA: 0x00019E6C File Offset: 0x0001806C
        [CommandLineFunctionality.CommandLineArgumentFunction("add_item_to_player_party", "cleancheats")]
        public static string AddItemToPlayerParty(List<string> strings)
        {
            string usage = "Format is \"cleancheats.add_item_to_player_party [ItemId] | [ModifierId] | [Amount]\"\n If amount is not entered only 1 item will be given.\n Modifier name is optional.";

            if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings))
            {
                return usage;
            }

            List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, false);
            ItemObject item = Game.Current.ObjectManager.GetObject<ItemObject>(separatedNames[0]);
            if (item == null)
            {
                return "Item is not found\n" + usage;
            }

            if (separatedNames.Count == 1)
            {
                PartyBase.MainParty.ItemRoster.AddToCounts(item, 1);
                return item.Name + " has been given to the main party.";
            }

            ItemModifier modifier = Game.Current.ObjectManager.GetObject<ItemModifier>(separatedNames[1]);
            if (modifier != null)
            {
                EquipmentElement rosterElement = new EquipmentElement(item, modifier, null, false);
                int amount = 1;
                if (separatedNames.Count > 2 && int.TryParse(separatedNames[2], out amount) && amount >= 1)
                {
                    if (amount > MaxAcceptableValue)
                    {
                        return "The value is too much";
                    }
                    MobileParty.MainParty.ItemRoster.AddToCounts(rosterElement, amount);
                }
                else
                {
                    MobileParty.MainParty.ItemRoster.AddToCounts(rosterElement, 1);
                }
                return rosterElement.GetModifiedItemName() + " has been given to the main party.";
            }

            if (!int.TryParse(separatedNames[1], out int plainAmount) || plainAmount < 1)
            {
                return "Second parameter is invalid.\n" + usage;
            }

            if (plainAmount > MaxAcceptableValue)
            {
                return "The value is too much";
            }

            MobileParty.MainParty.ItemRoster.AddToCounts(item, plainAmount);
            return item.Name + " has been given to the main party.";
        }
    }
}
using System.Collections.Generic;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.ObjectSystem;
using TaleWorlds.Library;

namespace CleanCheats
{
    // Token: 0x060003C9 RID: 969 RVA: 0x0001E178 File Offset: 0x0001C378
    public static class TroopCommand
    {
        private const float MaxAcceptableValue = 10000f;

        [CommandLineFunctionality.CommandLineArgumentFunction("add_troops", "cleancheats")]
        public static string AddTroopsToParty(List<string> strings)
        {
            if (CampaignCheats.CheckParameters(strings, 0))
            {
                return "Write \"cleancheats.add_troops help\" for help";
            }

            string usage = "Usage : \"cleancheats.add_troops [TroopId] | [Number]\". Always targets your own party.";
            List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, false);

            if (CampaignCheats.CheckHelp(strings) || separatedNames.Count < 2)
            {
                string helpText = usage + "\n\nAvailable troops\n==============================\n";
                foreach (CharacterObject characterObject in MBObjectManager.Instance.GetObjectTypeList<CharacterObject>())
                {
                    if (characterObject.Occupation == Occupation.Soldier || characterObject.Occupation == Occupation.Gangster)
                    {
                        helpText += $"Id: {characterObject.StringId} Name: {characterObject.Name}\n";
                    }
                }
                return helpText;
            }

            CampaignCheats.TryGetObject<CharacterObject>(separatedNames[0], out CharacterObject troop, out string error, null);
            if (troop == null)
            {
                return error + "\n" + usage;
            }

            if (troop.Occupation != Occupation.Soldier && troop.Occupation != Occupation.Gangster)
            {
                return "Troop occupation should be Soldier or Gangster to add party";
            }

            if (!int.TryParse(separatedNames[1], out int num) || num < 1)
            {
                return "Please enter a positive number\n" + usage;
            }

            MobileParty mobileParty = PartyBase.MainParty.MobileParty;

            if (mobileParty.MapEvent != null)
            {
                return "Party shouldn't be in a map event.";
            }

            if (num > MaxAcceptableValue)
            {
                return "The value is too much";
            }

            typeof(DefaultPartySizeLimitModel)
                .GetField("_addAdditionalPartySizeAsCheat", BindingFlags.Static | BindingFlags.NonPublic)
                ?.SetValue(null, true);

            mobileParty.AddElementToMemberRoster(troop, num, false);
            return $"{mobileParty.Name} gained {num} of {troop.Name}.";
        }
    }
}
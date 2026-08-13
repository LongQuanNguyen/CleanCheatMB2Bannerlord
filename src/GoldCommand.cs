using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Library;

namespace CleanCheats
{
    // Token: 0x06000397 RID: 919 RVA: 0x0001A384 File Offset: 0x00018584
    public static class GoldCommand
    {
        [CommandLineFunctionality.CommandLineArgumentFunction("add_gold", "cleancheats")]
        public static string AddGold(List<string> strings)
        {
            if (strings.Count == 0 || !int.TryParse(strings[0], out int amount))
            {
                return "Usage: cleancheats.add_gold [amount]";
            }

            GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, amount, true);
            return "Success";
        }
    }
}

using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Library;

namespace CleanCheats
{
    public static class RenownCommand
    {
      // Token: 0x06000396 RID: 918 RVA: 0x0001A258 File Offset: 0x00018458
        private const float MaxAcceptableValue = 10000f;

        [CommandLineFunctionality.CommandLineArgumentFunction("add_renown", "cleancheats")]
        public static string AddRenown(List<string> strings)
        {
            string usage = "Format is \"cleancheats.add_renown [PositiveNumber]\". If number is not specified, 100 will be added. Always targets your own clan.";

            if (CampaignCheats.CheckHelp(strings))
            {
                return usage;
            }

            int num = 100;
            if (!CampaignCheats.CheckParameters(strings, 0))
            {
                if (!int.TryParse(strings[0], out num))
                {
                    return "Please enter a positive number\n" + usage;
                }
            }

            if (num > MaxAcceptableValue)
            {
                return "The value is too much";
            }

            if (num <= 0)
            {
                return "Please enter a positive number\n" + usage;
            }

            Hero hero = Hero.MainHero;
            GainRenownAction.Apply(hero, num, false);
            return $"Added {num} renown to {hero.Clan.Name}";
        }
    }
}
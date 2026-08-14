using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;

namespace CleanCheats
{
    public static class HeroCommands
    {
        // Token: 0x0600037C RID: 892 RVA: 0x00017F34 File Offset: 0x00016134
        [CommandLineFunctionality.CommandLineArgumentFunction("set_hero_culture", "cleancheats")]
        public static string SetHeroCulture(List<string> strings)
        {
            string usage = "Format is \"cleancheats.set_hero_culture [HeroName] | [CultureName]\".";

            if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings))
            {
                return usage;
            }

            List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, false);
            if (separatedNames.Count != 2)
            {
                return usage;
            }

            if (!CampaignCheats.TryGetObject<CultureObject>(separatedNames[1], out CultureObject cultureObject, out string cultureError, null))
            {
                return cultureError + "\n" + usage;
            }

            if (!CampaignCheats.TryGetObject<Hero>(separatedNames[0], out Hero hero, out string heroError,
                    x => x.Occupation == Occupation.Lord || x.Occupation == Occupation.Wanderer))
            {
                return heroError + "\n" + usage;
            }

            if (hero.Culture == cultureObject)
            {
                return $"Hero culture is already {cultureObject.Name}";
            }

            hero.Culture = cultureObject;
            return "Success";
        }
    }
}
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
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

        // Token: 0x0600039A RID: 922 RVA: 0x0001A720 File Offset: 0x00018920
        [CommandLineFunctionality.CommandLineArgumentFunction("set_player_trait", "cleancheats")]
        public static string SetPlayerReputationTrait(List<string> strings)
        {
            string usage = "Format is \"cleancheats.set_player_trait [Trait] | [Number]\".";

            if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings))
            {
                return usage;
            }

            List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, true);
            if (separatedNames.Count != 2)
            {
                return usage;
            }

            if (!int.TryParse(separatedNames[1], out int num))
            {
                return "Please enter a number\n" + usage;
            }

            if (!CampaignCheats.TryGetObject<TraitObject>(separatedNames[0], out TraitObject traitObject, out string error, null))
            {
                return error + "\n" + usage;
            }

            if (num >= traitObject.MinValue && num <= traitObject.MaxValue)
            {
                Hero.MainHero.SetTraitLevel(traitObject, num);
                TraitLevelingHelper.UpdateTraitXPAccordingToTraitLevels();
                return $"Set {traitObject.Name} to {num}.";
            }

            return $"Number must be between {traitObject.MinValue} and {traitObject.MaxValue}.";
        }

        // Token: 0x060003A1 RID: 929 RVA: 0x0001B104 File Offset: 0x00019304
        [CommandLineFunctionality.CommandLineArgumentFunction("marry_hero_to_hero", "cleancheats")]
        public static string MarryHeroWithHero(List<string> strings)
        {
            string usage = "Format is \"cleancheats.marry_hero_to_hero [HeroName] | [HeroName]\".";
 
            List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, false);
            if (separatedNames.Count != 2 || CampaignCheats.CheckHelp(strings))
            {
                return usage;
            }
 
            CampaignCheats.TryGetObject<Hero>(separatedNames[0], out Hero hero, out string error1, null);
            CampaignCheats.TryGetObject<Hero>(separatedNames[1], out Hero hero2, out string error2, null);
 
            if (hero == null)
            {
                return error1 + "\nCan't find a hero with name: " + separatedNames[0];
            }
 
            if (hero2 == null)
            {
                return error2 + "\nCan't find a hero with name: " + separatedNames[1];
            }
 
            if (Campaign.Current.Models.MarriageModel.IsCoupleSuitableForMarriage(hero, hero2))
            {
                MarriageAction.Apply(hero, hero2, true);
                return "Success";
            }
 
            return "They are not suitable for marriage";
        }

    }
}
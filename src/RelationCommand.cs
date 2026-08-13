using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace CleanCheats
{
    public static class RelationCommand
    {
      // Token: 0x0600038E RID: 910 RVA: 0x00019954 File Offset: 0x00017B54
        private const float MaxAcceptableValue = 10000f;

        [CommandLineFunctionality.CommandLineArgumentFunction("add_hero_relation", "cleancheats")]
        public static string AddHeroRelation(List<string> strings)
        {
            bool isDevelopmentMode = Game.Current.IsDevelopmentMode;
            string usage = isDevelopmentMode
                ? "Format is \"cleancheats.add_hero_relation [HeroName]/All | [OtherHeroName(optional)] | [Value] \".\n"
                : "Format is \"cleancheats.add_hero_relation [HeroName] | [OtherHeroName(optional)] | [Value] \".\n";

            if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings))
            {
                return usage;
            }

            Hero mainHero = Hero.MainHero;
            string valueString;
            List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, false);

            if (separatedNames.Count == 3)
            {
                if (!CampaignCheats.TryGetObject<Hero>(separatedNames[1], out mainHero, out string result, null))
                {
                    return result;
                }
                valueString = separatedNames[2];
            }
            else
            {
                if (separatedNames.Count != 2)
                {
                    return usage;
                }
                valueString = separatedNames[1];
            }

            if (!int.TryParse(valueString, out int num))
            {
                return "Please enter a number\n" + usage;
            }

            if (num > MaxAcceptableValue)
            {
                return "The value is too much";
            }

            string requestedId = separatedNames[0];
            CampaignCheats.TryGetObject<Hero>(requestedId, out Hero hero, out string error, null);

            if (hero == mainHero)
            {
                return "Can not add relation to same heroes.";
            }

            if (hero != null)
            {
                ChangeRelationAction.ApplyRelationChangeBetweenHeroes(hero, mainHero, num, true);
                return "Success";
            }

            if (string.Equals(requestedId, "all", StringComparison.OrdinalIgnoreCase) && isDevelopmentMode)
            {
                foreach (Hero otherHero in Hero.AllAliveHeroes)
                {
                    if (!otherHero.IsHumanPlayerCharacter && otherHero != mainHero)
                    {
                        ChangeRelationAction.ApplyRelationChangeBetweenHeroes(otherHero, mainHero, num, true);
                    }
                }
                return "Success";
            }

            return error + "\n" + usage;
        }
    }
}
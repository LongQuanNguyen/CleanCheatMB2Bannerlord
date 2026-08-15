using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace CleanCheats
{
    public static class SkillCommand
    {
      // Token: 0x0600037E RID: 894 RVA: 0x000180CC File Offset: 0x000162CC
        [CommandLineFunctionality.CommandLineArgumentFunction("add_skill_xp_to_hero", "cleancheats")]
        public static string AddSkillXpToHero(List<string> strings)
        {
            Hero mainHero = Hero.MainHero;
            string usage = "Format is \"cleancheats.add_skill_xp_to_hero [HeroName] | [SkillName] | [PositiveNumber]\".";

            if (CampaignCheats.CheckHelp(strings))
            {
                return usage;
            }

            if (CampaignCheats.CheckParameters(strings, 0))
            {
                // Zero arguments: default 100 xp to every skill of the main hero.
                return ApplyXpToAllSkills(mainHero, 100);
            }

            List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, true);

            if (separatedNames.Count == 1)
            {
                return HandleSingleArgument(separatedNames[0], mainHero, usage);
            }

            if (separatedNames.Count == 2)
            {
                return HandleTwoArguments(separatedNames[0], separatedNames[1], mainHero, usage);
            }

            if (separatedNames.Count == 3)
            {
                return HandleThreeArguments(separatedNames[0], separatedNames[1], separatedNames[2], usage);
            }

            return usage;
        }

        private static string HandleSingleArgument(string arg, Hero mainHero, string usage)
        {
            if (int.TryParse(arg, out int amount))
            {
                if (amount <= 0)
                {
                    return "Please enter a positive number\n" + usage;
                }
                return ApplyXpToAllSkills(mainHero, amount);
            }

            // Not a number - could be a hero name or a skill name.
            CampaignCheats.TryGetObject<Hero>(arg, out Hero targetHero, out _, null);
            if (targetHero != null)
            {
                return ApplyXpToAllSkills(targetHero, 100);
            }

            SkillObject? skill = FindSkillByNameOrId(arg);
            if (skill != null)
            {
                return ApplySkillXp(mainHero, skill, 100);
            }

            return usage;
        }

        private static string HandleTwoArguments(string first, string second, Hero mainHero, string usage)
        {
            CampaignCheats.TryGetObject<Hero>(first, out Hero targetHero, out _, null);

            if (targetHero != null)
            {
                if (int.TryParse(second, out int amount))
                {
                    if (amount <= 0)
                    {
                        return "Please enter a positive number\n" + usage;
                    }

                    SkillObject firstSkill = Skills.All.FirstOrDefault();
                    return firstSkill == null ? usage : ApplySkillXp(targetHero, firstSkill, amount);
                }

                SkillObject? namedSkill = FindSkillByNameOrId(second);
                return namedSkill == null
                    ? "Skill not found.\n" + usage
                    : ApplySkillXp(targetHero, namedSkill, 100);
            }

            // Hero not found - treat as [SkillName] | [Number] against the main hero.
            if (!int.TryParse(second, out int fallbackAmount))
            {
                return usage;
            }

            if (fallbackAmount <= 0)
            {
                return "Please enter a positive number\n" + usage;
            }

            SkillObject? skill = FindSkillByNameOrId(first);
            return skill == null
                ? "Skill not found.\n" + usage
                : ApplySkillXp(mainHero, skill, fallbackAmount);
        }

        private static string HandleThreeArguments(string heroName, string skillName, string amountText, string usage)
        {
            if (!int.TryParse(amountText, out int amount) || amount < 0)
            {
                return "Please enter a positive number\n" + usage;
            }

            CampaignCheats.TryGetObject<Hero>(heroName, out Hero targetHero, out string heroError, null);
            if (targetHero == null)
            {
                return heroError + "\n" + usage;
            }

            SkillObject? skill = FindSkillByNameOrId(skillName);
            return skill == null
                ? "Skill not found.\n" + usage
                : ApplySkillXp(targetHero, skill, amount);
        }

        private static SkillObject? FindSkillByNameOrId(string text)
        {
            string normalized = text.Replace(" ", "");
            foreach (SkillObject skill in Skills.All)
            {
                string skillName = skill.Name.ToString().Replace(" ", "");
                string skillId = skill.StringId.Replace(" ", "");
                if (skillName.Equals(normalized, StringComparison.InvariantCultureIgnoreCase)
                    || skillId.Equals(normalized, StringComparison.InvariantCultureIgnoreCase))
                {
                    return skill;
                }
            }
            return null;
        }

        private static string ApplySkillXp(Hero hero, SkillObject skill, int amount)
        {
            if (hero.GetSkillValue(skill) >= 300)
            {
                return $"{skill.Name} value for {hero.Name} is already at max.";
            }

            hero.HeroDeveloper.AddSkillXp(skill, amount, true, true);
            int adjusted = (int)(hero.HeroDeveloper.GetFocusFactor(skill) * amount);
            return $"Input {amount} xp is modified to {adjusted} xp due to focus point factor \nand added to the {hero.Name}'s {skill.Name} skill.";
        }

        private static string ApplyXpToAllSkills(Hero hero, int amount)
        {
            var sb = new StringBuilder();
            foreach (SkillObject skill in Skills.All)
            {
                hero.HeroDeveloper.AddSkillXp(skill, amount, true, true);
                int adjusted = (int)(hero.HeroDeveloper.GetFocusFactor(skill) * amount);
                sb.AppendLine($"{amount} xp is modified to {adjusted} xp due to focus point factor \nand added to the {hero.Name}'s {skill.Name} skill.");
            }
            return sb.ToString();
        }
    }
}
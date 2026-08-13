using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace CleanCheats
{
    // Token: 0x060003B4 RID: 948 RVA: 0x0001C5F0 File Offset: 0x0001A7F0
    public static class ConceiveChildCommand
    {
        [CommandLineFunctionality.CommandLineArgumentFunction("conceive_child", "cleancheats")]
        public static string MakePregnant(List<string> strings)
        {
            if (Hero.MainHero.Spouse == null)
            {
                if (!Game.Current.IsDevelopmentMode)
                {
                    return "You need to be married to have a child.";
                }

                Hero hero = Hero.AllAliveHeroes.FirstOrDefault(
                    t => t != Hero.MainHero && Campaign.Current.Models.MarriageModel.IsCoupleSuitableForMarriage(Hero.MainHero, t));
                if (hero == null)
                {
                    return "error";
                }

                MarriageAction.Apply(Hero.MainHero, hero, true);
                if (Hero.MainHero.IsFemale ? !Hero.MainHero.IsPregnant : !Hero.MainHero.Spouse.IsPregnant)
                {
                    MakePregnantAction.Apply(Hero.MainHero.IsFemale ? Hero.MainHero : Hero.MainHero.Spouse);
                    return "Success";
                }

                return "You are expecting a child already.";
            }

            if (Hero.MainHero.IsFemale ? !Hero.MainHero.IsPregnant : !Hero.MainHero.Spouse.IsPregnant)
            {
                MakePregnantAction.Apply(Hero.MainHero.IsFemale ? Hero.MainHero : Hero.MainHero.Spouse);
                return "Success";
            }

            return "You are expecting a child already.";
        }
    }
}
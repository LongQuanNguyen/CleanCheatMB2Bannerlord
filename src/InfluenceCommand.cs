using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Library;

namespace CleanCheats
{  
    // Token: 0x06000395 RID: 917 RVA: 0x0001A1B0 File Offset: 0x000183B0
    public static class InfluenceCommand
    {
        private const float MaxAcceptableValue = 10000f;

        [CommandLineFunctionality.CommandLineArgumentFunction("add_influence", "cleancheats")]
        public static string AddInfluence(List<string> strings)
        {
            if (CampaignCheats.CheckHelp(strings))
            {
                return "Format is \"cleancheats.add_influence [Number]\". If Number is not entered, 100 influence will be added.";
            }

            int num = 100;
            bool flag = false;
            if (!CampaignCheats.CheckParameters(strings, 0))
            {
                flag = int.TryParse(strings[0], out num);
            }

            if (num > MaxAcceptableValue)
            {
                return "The value is too much";
            }

            if (flag || CampaignCheats.CheckParameters(strings, 0))
            {
                float num2 = MBMath.ClampFloat((float)num, -200f, float.MaxValue);
                ChangeClanInfluenceAction.Apply(Clan.PlayerClan, num2);
                return string.Format("The influence of player is changed by {0} to {1} ", num2, Clan.PlayerClan.Influence);
            }

            return "Please enter a positive number\nFormat is \"cleancheats.add_influence [Number]\".";
        }
    }
}
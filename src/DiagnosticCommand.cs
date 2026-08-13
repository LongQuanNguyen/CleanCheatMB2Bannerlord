using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;

namespace CleanCheats
{
    public static class DiagnosticCommand
    {
        private const BindingFlags PrivateInstance = BindingFlags.NonPublic | BindingFlags.Instance;

        [CommandLineFunctionality.CommandLineArgumentFunction("check_taint", "cleancheats")]
        public static string CheckTaint(List<string> strings)
        {
            if (Campaign.Current == null)
            {
                return "No active campaign.";
            }

            var sb = new StringBuilder();
            sb.AppendLine("=== Achievement/Integrity Status ===");

            bool cheatModeLive = Game.Current.CheatMode;
            sb.AppendLine(cheatModeLive
                ? "cheat_mode (live): ON - will re-taint EnabledCheatsBefore on every check"
                : "cheat_mode (live): off");

            bool enabledCheatsBefore = Campaign.Current.EnabledCheatsBefore;
            sb.AppendLine(enabledCheatsBefore
                ? "EnabledCheatsBefore (save flag): TAINTED"
                : "EnabledCheatsBefore (save flag): clean");

            AppendModuleHistoryStatus(sb);
            AppendVersionHistoryStatus(sb);

            sb.AppendLine();
            sb.AppendLine("Does not check StoryMode's AchievementsCampaignBehavior._deactivateAchievements, since this mod has no StoryMode dependency. Check Achievement Patch's own log for that flag.");

            return sb.ToString();
        }

        private static void AppendModuleHistoryStatus(StringBuilder sb)
        {
            var modulesField = typeof(Campaign).GetField("_previouslyUsedModules", PrivateInstance);
            var modulesList = modulesField?.GetValue(Campaign.Current) as MBList<string>;
            string? lastSlug = modulesList?.LastOrDefault();

            if (lastSlug == null)
            {
                sb.AppendLine("Module history: unavailable (field may have been renamed, check dnSpy)");
                return;
            }

            var officialIds = ModuleHelper.GetOfficialModuleIds();
            var unofficial = lastSlug
                .Split(MBSaveLoad.ModuleCodeSeperator)
                .Select(entry => entry.Split(MBSaveLoad.ModuleVersionSeperator)[0])
                .Where(id => !officialIds.Any(officialId => string.Equals(officialId, id, System.StringComparison.OrdinalIgnoreCase)))
                .Distinct()
                .ToList();

            sb.AppendLine(unofficial.Count == 0
                ? "Module history: clean"
                : $"Module history: TAINTED, unofficial modules recorded: {string.Join(", ", unofficial)}");
        }

        private static void AppendVersionHistoryStatus(StringBuilder sb)
        {
            var versionsField = typeof(Campaign).GetField("_usedGameVersions", PrivateInstance);
            var versionsList = versionsField?.GetValue(Campaign.Current) as MBList<string>;

            if (versionsList == null)
            {
                sb.AppendLine("Version history: unavailable (field may have been renamed, check dnSpy)");
                return;
            }

            sb.AppendLine(versionsList.Count <= 1
                ? "Version history: clean (single version recorded)"
                : $"Version history: {versionsList.Count} versions recorded, check for a downgrade");
        }
    }
}
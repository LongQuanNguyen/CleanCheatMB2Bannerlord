using System;
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
    public static class DiagnosticCommands
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
            AppendAchievementFlagStatus(sb);

            return sb.ToString();
        }

        [CommandLineFunctionality.CommandLineArgumentFunction("clear_taint", "cleancheats")]
        public static string ClearTaint(List<string> strings)
        {
            if (Campaign.Current == null)
            {
                return "No active campaign.";
            }

            if (Game.Current.CheatMode)
            {
                return "cheat_mode is currently ON. Clearing now would be undone immediately - "
                     + "the game re-taints EnabledCheatsBefore on every check while cheat_mode stays on, "
                     + "so nothing done here would stick.\n"
                     + "Run \"config.cheat_mode 0\" in the console first, then run cleancheats.clear_taint again.";
            }

            var sb = new StringBuilder();
            sb.AppendLine("=== Clearing Taint ===");

            TaintCorrection.ClearCheatFlag();
            sb.AppendLine("Cleared EnabledCheatsBefore.");

            bool historyCorrected = TaintCorrection.ClearModuleAndVersionHistory();
            sb.AppendLine(historyCorrected
                ? "Cleared module/version history."
                : "Module/version history: nothing to clear or fields unavailable.");

            bool? achievementResult = TaintCorrection.ClearAchievementDeactivationFlag();
            sb.AppendLine(achievementResult switch
            {
                null => "Achievement deactivation flag: not applicable (StoryMode not loaded).",
                true => "Cleared achievement deactivation flag.",
                false => "Achievement deactivation flag: unavailable (behavior or field not found)."
            });

            sb.AppendLine();
            sb.AppendLine("Done. Run cleancheats.check_taint to confirm.");

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

        private static void AppendAchievementFlagStatus(StringBuilder sb)
        {
            Type? achievementsType = TaintCorrection.ResolveAchievementsType();

            if (achievementsType == null)
            {
                sb.AppendLine("Achievement deactivation flag: not applicable (StoryMode not loaded)");
                return;
            }

            MethodInfo? genericMethod = typeof(Campaign).GetMethod("GetCampaignBehavior", BindingFlags.Public | BindingFlags.Instance);
            object? behaviorInstance = genericMethod?.MakeGenericMethod(achievementsType).Invoke(Campaign.Current, null);

            if (behaviorInstance == null)
            {
                sb.AppendLine("Achievement deactivation flag: unavailable (behavior instance not found)");
                return;
            }

            FieldInfo? field = achievementsType.GetField("_deactivateAchievements", PrivateInstance);
            if (field == null)
            {
                sb.AppendLine("Achievement deactivation flag: unavailable (field may have been renamed, check dnSpy)");
                return;
            }

            bool deactivated = (bool)field.GetValue(behaviorInstance);
            sb.AppendLine(deactivated
                ? "Achievement deactivation flag: TAINTED (latches permanently until corrected)"
                : "Achievement deactivation flag: clean");
        }
    }
}
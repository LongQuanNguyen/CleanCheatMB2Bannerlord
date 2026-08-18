using System;
using System.Linq;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;

namespace CleanCheats
{
    internal static class TaintCorrection
    {
        internal const BindingFlags PrivateInstance = BindingFlags.NonPublic | BindingFlags.Instance;
        internal static void ClearCheatFlag()
        {
            if (Campaign.Current != null)
            {
                Campaign.Current.EnabledCheatsBefore = false;
            }
        }

        /// <summary>
        /// Filters Campaign._previouslyUsedModules down to official module
        /// IDs only, and collapses _usedGameVersions to the current version.
        /// Returns true if it found and corrected something, false if
        /// reflection failed (field renamed) or there was nothing to do.
        /// </summary>
        internal static bool ClearModuleAndVersionHistory()
        {
            if (Campaign.Current == null)
            {
                return false;
            }

            bool corrected = false;
            var campaignType = typeof(Campaign);

            var modulesField = campaignType.GetField("_previouslyUsedModules", PrivateInstance);
            var modulesList = modulesField?.GetValue(Campaign.Current) as MBList<string>;
            if (modulesList != null)
            {
                string lastSlug = modulesList.LastOrDefault();
                modulesList.Clear();

                if (lastSlug != null)
                {
                    var officialIds = ModuleHelper.GetOfficialModuleIds();
                    var officialEntries = lastSlug
                        .Split(MBSaveLoad.ModuleCodeSeperator)
                        .Where(entry =>
                        {
                            string moduleId = entry.Split(MBSaveLoad.ModuleVersionSeperator)[0];
                            return officialIds.Any(id => string.Equals(id, moduleId, StringComparison.OrdinalIgnoreCase));
                        });

                    string cleanedSlug = string.Join(MBSaveLoad.ModuleCodeSeperator.ToString(), officialEntries);
                    if (!string.IsNullOrEmpty(cleanedSlug))
                    {
                        modulesList.Add(cleanedSlug);
                    }
                }
                corrected = true;
            }

            var versionsField = campaignType.GetField("_usedGameVersions", PrivateInstance);
            var versionsList = versionsField?.GetValue(Campaign.Current) as MBList<string>;
            if (versionsList != null)
            {
                versionsList.Clear();
                versionsList.Add(MBSaveLoad.CurrentVersion.ToString());
                corrected = true;
            }

            return corrected;
        }

        /// <summary>
        /// Resolves StoryMode's AchievementsCampaignBehavior by scanning
        /// loaded assemblies rather than Type.GetType, since Bannerlord's
        /// module loader can put it in a different AssemblyLoadContext than
        /// this mod's own assembly
        /// </summary>
        internal static Type? ResolveAchievementsType()
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.GetName().Name == "StoryMode")
                {
                    return assembly.GetType("StoryMode.GameComponents.CampaignBehaviors.AchievementsCampaignBehavior");
                }
            }

            return null;
        }

        /// <summary>
        /// Clears AchievementsCampaignBehavior._deactivateAchievements.
        /// Returns null if not applicable (StoryMode not loaded), true if
        /// found and cleared, false if the behavior/field couldn't be
        /// resolved even though StoryMode is loaded (likely renamed).
        /// </summary>
        internal static bool? ClearAchievementDeactivationFlag()
        {
            Type? achievementsType = ResolveAchievementsType();
            if (achievementsType == null)
            {
                return null;
            }

            MethodInfo? genericMethod = typeof(Campaign).GetMethod("GetCampaignBehavior", BindingFlags.Public | BindingFlags.Instance);
            object? behaviorInstance = genericMethod?.MakeGenericMethod(achievementsType).Invoke(Campaign.Current, null);
            if (behaviorInstance == null)
            {
                return false;
            }

            FieldInfo? field = achievementsType.GetField("_deactivateAchievements", PrivateInstance);
            if (field == null)
            {
                return false;
            }

            field.SetValue(behaviorInstance, false);
            return true;
        }
    }
}
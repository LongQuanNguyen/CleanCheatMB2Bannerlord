using System.Linq;
using System.Reflection;
using TaleWorlds.Core;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;

namespace CleanCheats
{
    /// <summary>
    /// Corrects Campaign._previouslyUsedModules so CleanCheats's own presence
    /// doesn't trip DumpIntegrityCampaignBehavior.CheckIfModulesAreDefault.
    ///

    /// CheckIfModulesAreDefault reads Campaign.PreviouslyUsedModules, a list
    /// written once per session by DetermineSavedStats during
    /// Campaign.OnInitialize. There's no continuous re-taint race here, so a
    /// single correction after that point should hold for the rest of the
    /// session.
    /// </summary>
    public class ModuleHistoryGuard : CampaignBehaviorBase
    {
        private const BindingFlags PrivateInstance = BindingFlags.NonPublic | BindingFlags.Instance;

        public override void RegisterEvents()
        {
            CampaignEvents.OnGameLoadFinishedEvent.AddNonSerializedListener(this, FixModuleHistory);
            CampaignEvents.OnNewGameCreatedPartialFollowUpEndEvent.AddNonSerializedListener(this, (CampaignGameStarter _) => FixModuleHistory());
            CampaignEvents.OnConfigChangedEvent.AddNonSerializedListener(this, FixModuleHistory);
        }

        public override void SyncData(IDataStore dataStore)
        {
        }

        private void FixModuleHistory()
        {
            if (Campaign.Current == null)
            {
                return;
            }

            var campaignType = typeof(Campaign);
            var modulesField = campaignType.GetField("_previouslyUsedModules", PrivateInstance);
            var modulesList = modulesField?.GetValue(Campaign.Current) as MBList<string>;
            if (modulesList == null)
            {
                return;
            }

            string lastSlug = modulesList.LastOrDefault();
            modulesList.Clear();

            if (lastSlug == null)
            {
                return;
            }

            var officialIds = ModuleHelper.GetOfficialModuleIds();
            var officialEntries = lastSlug
                .Split(MBSaveLoad.ModuleCodeSeperator)
                .Where(entry =>
                {
                    string moduleId = entry.Split(MBSaveLoad.ModuleVersionSeperator)[0];
                    return officialIds.Any(id => string.Equals(id, moduleId, System.StringComparison.OrdinalIgnoreCase));
                });

            string cleanedSlug = string.Join(MBSaveLoad.ModuleCodeSeperator.ToString(), officialEntries);
            if (!string.IsNullOrEmpty(cleanedSlug))
            {
                modulesList.Add(cleanedSlug);
            }
        }
    }
}

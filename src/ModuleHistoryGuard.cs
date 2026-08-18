using TaleWorlds.CampaignSystem;

namespace CleanCheats
{
    public class ModuleHistoryGuard : CampaignBehaviorBase
    {
        public override void RegisterEvents()
        {
            CampaignEvents.OnGameLoadFinishedEvent.AddNonSerializedListener(this, () => TaintCorrection.ClearModuleAndVersionHistory());
            CampaignEvents.OnNewGameCreatedPartialFollowUpEndEvent.AddNonSerializedListener(this, (CampaignGameStarter _) => TaintCorrection.ClearModuleAndVersionHistory());
            CampaignEvents.OnConfigChangedEvent.AddNonSerializedListener(this, () => TaintCorrection.ClearModuleAndVersionHistory());
        }

        public override void SyncData(IDataStore dataStore)
        {
        }
    }
}
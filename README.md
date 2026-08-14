# Clean Cheats

Console commands that replicate common Bannerlord cheat effects without
ever setting `cheat_mode`, so achievements stay unlockable.

## Why this exists

Vanilla cheat commands (`campaign.add_gold_to_hero`, etc.) all call
`CampaignCheats.CheckCheatUsage`, which requires `cheat_mode` to be on.
Once on, `DumpIntegrityCampaignBehavior.CheckCheatUsage()` re-taints
`Campaign.EnabledCheatsBefore` on every check, permanently, for as long as
`cheat_mode` stays enabled - no reflection-based fix can outrun that, since
it re-derives the taint from live engine state rather than storing a
one-time flag. 

Each command provided by this mod calls the same underlying game action a vanilla cheat would but skips the `CheckCheatUsage` gate entirely, so `cheat_mode` never
needs to be touched.

## Installing

Copy `dist\CleanCheats\` into `<BannerlordDir>\Modules\CleanCheats\`.

## Enabling

Enable "Clean Cheats" in the Bannerlord Launcher's Mods tab. Use commands
via the in-game console (Alt + `~`) - no `cheat_mode` toggle needed.

## Commands

| Command | Effect |
|---|---|
| `cleancheats.add_gold` | Adds gold to the main hero |
| `cleancheats.add_influence` | Adds influence to the player's clan |
| `cleancheats.add_troops` | Adds troops to your own party |
| `cleancheats.add_renown` | Adds renown to the player's own clan |
| `cleancheats.conceive_child` | Starts a pregnancy for the player/spouse |
| `cleancheats.set_hero_culture` | Sets a lord or wanderer's culture |
| `cleancheats.set_loyalty_of_settlement` | Sets a town/castle's loyalty (0-100) |
| `cleancheats.set_prosperity_of_settlement` | Sets a town/castle's prosperity |
| `cleancheats.set_militia_of_settlement` | Sets a settlement's militia |
| `cleancheats.set_security_of_settlement` | Sets a town/castle's security |
| `cleancheats.set_food_of_settlement` | Sets a town/castle's food stocks |
| `cleancheats.set_hearth_of_settlement` | Sets a village's hearth |
| `cleancheats.add_building_level` | Increases a building's level by 1 |
| `cleancheats.check_taint` | Reports current cheat/module/version taint status |


More can be added by finding the vanilla command's implementation
in `TaleWorlds.CampaignSystem.CampaignCheats`, via dnSpy inspection on "~\SteamLibrary\steamapps\common\Mount & Blade II Bannerlord\bin\Win64_Shipping_Client\TaleWorlds.CampaignSystem.dll". Copy
and drop the `CheckCheatUsage` guard  clause.

## Self-tainting by presence

Being a third-party module trips the official-module check
(`CheckIfModulesAreDefault`) on its own, regardless of whether any command
is used. This mod also patches that check, so its own presence doesn't
taint the save or disable achievements.

## Building (optional)

```powershell
dotnet build -c Release -p:BannerlordDir="C:\Program Files (x86)\Steam\steamapps\common\Mount & Blade II Bannerlord"
```

Output goes to `dist\CleanCheats\`

## Built/tested against

Bannerlord v1.4.8, `netstandard2.0`.

## License

MIT, see [LICENSE](LICENSE).

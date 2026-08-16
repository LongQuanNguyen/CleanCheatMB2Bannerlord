# Clean Cheats

Console commands that replicate common Bannerlord cheat effects without
ever setting `cheat_mode`, so achievements stay unlockable.

## Why this exists

Every time a vanilla cheat command is called, the game checks whether the ```cheat_mode``` setting is on. If ``cheat_mode`` is on, the check also flags the save file as tainted, making achievements unavailable for that save from that point on.

Each command in this mod does the same thing a vanilla cheat would, but skips that check entirely, so the save never gets flagged, and cheat_mode never needs to be turned on.

## Installing

Copy `dist\CleanCheats\` into `<BannerlordDir>\Modules\`.

## Enabling

Enable "Clean Cheats" in the Bannerlord Launcher's Mods tab. 

## Using
Use commands via the in-game console (Alt + `~`).

## Commands

| Command | Effect |
|---|---|
| `cleancheats.add_gold` | Adds gold to the main hero |
| `cleancheats.add_influence` | Adds influence to the player's clan |
| `cleancheats.add_troops` | Adds troops to your own party |
| `cleancheats.add_renown` | Adds renown to the player's own clan |
| `cleancheats.conceive_child` | Starts a pregnancy for the player/spouse |
| `cleancheats.set_hero_culture` | Sets a lord or wanderer's culture |
| `cleancheats.set_player_trait` | Sets one of the player's reputation traits |
| `cleancheats.marry_hero_to_hero` | Marries two heroes, if suitable |
| `cleancheats.add_skill_xp_to_hero` | Adds skill xp to a hero |
| `cleancheats.add_focus_points_to_hero` | Adds focus points to a hero |
| `cleancheats.add_attribute_points_to_hero` | Adds attribute points to a hero |
| `cleancheats.set_loyalty_of_settlement` | Sets a town/castle's loyalty (0-100) |
| `cleancheats.set_prosperity_of_settlement` | Sets a town/castle's prosperity |
| `cleancheats.set_militia_of_settlement` | Sets a settlement's militia |
| `cleancheats.set_security_of_settlement` | Sets a town/castle's security |
| `cleancheats.set_food_of_settlement` | Sets a town/castle's food stocks |
| `cleancheats.set_hearth_of_settlement` | Sets a village's hearth |
| `cleancheats.add_building_level` | Increases a building's level by 1 |
| `cleancheats.check_taint` | Reports current cheat/module/version taint status |



More can be added by finding and re-implement the vanilla command's implementation
in `TaleWorlds.CampaignSystem.CampaignCheats`, by dnSpy inspection on `<BannerlordDir>\bin\Win64_Shipping_Client\TaleWorlds.CampaignSystem.dll`.

## Self-tainting by presence

Being a third-party module trips the official-module check
(`CheckIfModulesAreDefault`) on its own, regardless of whether any command
is used. This mod also patches that check, so its presence doesn't
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

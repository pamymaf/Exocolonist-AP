This is a mod that adds Archipelago support to I Was a Teenage Exocolonist. Both the apworld and game mod are included.

# Exocolonist Setup Guide

## Requirements

[Exoloader](https://github.com/Pandemonium14/ExoLoader/releases)
[Exopelago](https://github.com/pamymaf/Exocolonist-AP/releases)

## Instructions

### Windows

1. Download Exoloader and extract it.
2. Copy the contents of the Exocolonist folder into your Exocolonist game directory.
3. Download Exopelago and extract it.
4. Copy the contents of the Exoloader folder into the CustomContent folder.
  The file structure should be `Exocolonist/CustomContent/Exopelago/`
5. Copy `Exopelago.dll`, `Archipelago.Multiclient.net.dll`, and `Newtonsoft.json.dll` into the `BepInEx/plugins/` folder.
6. Copy `connectionInfo.json` into your Exocolonist folder next to `Exocolonist.exe`.
7. Edit `connectionInfo.json` with your server, port, slot name, and password.
8. Launch the game and enjoy!

### Linux

1. Tell Steam to use compatibility mode.
  This game does have a native Linux release, but BeInEx needs the Windows release.
2. Launch the game at least once to build the Proton prefix.
3. Install and open Protontricks.
  If needed, select the version of Steam you want to use (native vs flatpak).
4. Select the game.
  It may take Protontricks a couple minutes to open the next window.
5. Click Select the default wineprifix and click OK.
6. Select Run winecfg and click OK.
7. Navigate to the Libraries tab.
8. In the textbox, enter winhttp and click Add.
9. Click OK at the bottom.
10. Follow Windows instructions and enjoy!

# Exocolonist Randomizer

Exocolonist is a visual novel card battler time loop game about growing up on an alien planet. You have 10 years to survive, hopefully making friendships and lasting memories along the way.

# What's randomized?

## Locations

Every job you can unlock is a location.

### Friendsanity

If enabled, each multiple of 20 friendship points for each character is a check.

### Datesanity

If enabled, dating each character is a check.

### Perksanity

If enabled, each skill perk is a check. Additionally, skills will be locked so you cannot progress past a perk you don't have. As an example, if you have perception perk 1 you can level your perception to 66. But if you have no perks, you can only level the skill to 33.

### Special events

Right now the special events are
  - Save Tammy
  - Save Tonin
  - Rescue Eudicot
  - Save Mom
  - Save Dad
  - Save Hal
  - Adopt Vriki
  - Adopt Hopeye
  - Adopt Robot
  - Adopt Unisaur
  - Become Governor
  - Help Marz become Governor


## Items

You can receive job unlocks and consumables to use in card battles or as gifts for characters. If perksanity is enabled, you will also receive progressive skill perks.

### Progressive Years

In order to age up you will need to receive a progressive year. For example, if you only have 3 progressive years the game will end (but not goal) when you age up to 14.
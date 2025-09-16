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
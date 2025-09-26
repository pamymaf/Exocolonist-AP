using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Northway.Utils;
using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Linq;
using Exopelago.Archipelago;

namespace Exopelago;

[HarmonyPatch]
class SavePatch
{
  [HarmonyPatch(typeof(Savegame), "Load2")]
  [HarmonyPostfix]
  public static void Load2Postfix(Savegame __instance)
  {
    string connectedSeed = ArchipelagoClient.serverData.seed;
    string connectedSlot = ArchipelagoClient.serverData.slotName;
    JObject saveJson = Helpers.GetConnectionInfoSaveGame();
    Helpers.firstMapLoad = true;
    Plugin.Logger.LogInfo($"Loaded save. Save info: {saveJson}");
    if (saveJson == null) {
      Helpers.DisplayConnectionMessage("This is a vanilla save");
    } else if (connectedSeed == (string)saveJson["seed"] && connectedSlot == (string)saveJson["slot"]){
      ArchipelagoClient.serverData.InitializeData();
      Helpers.DisplayConnectionMessage("Refreshing unlocks");
      ArchipelagoClient.RefreshUnlocks(true);
    } else {
      Helpers.DisplayConnectionMessage("Invalid seed and slot name");
    }
  }
}
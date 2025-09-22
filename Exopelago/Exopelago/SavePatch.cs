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

[HarmonyPatch(typeof(Savegame))]
class Save_ExecutePatch
{
  [HarmonyPatch("Load")]
  [HarmonyPrefix]
  public static void Prefix(Savegame __instance)
  {
  }

  [HarmonyPatch("Load2")]
  [HarmonyPostfix]
  public static void Postfix(Savegame __instance)
  {
    string connectedSeed = ArchipelagoClient.serverData.seed;
    string connectedSlot = ArchipelagoClient.serverData.slotName;
    JObject saveJson = Helpers.GetConnectionInfoSaveGame();
    if (connectedSeed == (string)saveJson["apSeed"] && connectedSlot == (string)saveJson["apSlot"]){
      Helpers.firstMapLoad = true;
      ArchipelagoClient.serverData.InitializeData();
      Plugin.Logger.LogInfo("Refreshing unlocks");
      ArchipelagoClient.RefreshUnlocks(true);
    } else {
      Helpers.DisplayConnectionMessage("Invalid seed and slot name");
    }
  }
}
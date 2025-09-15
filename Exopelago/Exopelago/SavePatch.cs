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
  [HarmonyPostfix]
  public static void Prefix(Savegame __instance)
  {
  }

  [HarmonyPatch("Load2")]
  [HarmonyPostfix]
  public static void Postfix(Savegame __instance)
  {
    try {
      // Just in case we're already connected
      ArchipelagoClient.Disconnect();
    }
    catch {}
    JObject json = Helpers.GetConnectionInfoSaveGame();
    Helpers.Connect(json);
    Plugin.Logger.LogInfo("Refreshing unlocks");
    ArchipelagoClient.RefreshUnlocks();
  }
}
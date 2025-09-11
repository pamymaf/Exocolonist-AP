using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Northway.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using Exopelago.Archipelago;

namespace Exopelago;

[HarmonyPatch(typeof(Savegame))]
class Save_ExecutePatch
{
  [HarmonyPatch("Load2")]
  [HarmonyPostfix]
  public static void Postfix(Savegame __instance)
  {
    Plugin.Logger.LogInfo("Refreshing unlocks");
    ArchipelagoClient.RefreshUnlocks();
  }
}
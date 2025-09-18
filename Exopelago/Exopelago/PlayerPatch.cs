using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Northway.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using Exopelago.Archipelago;

namespace Exopelago;

[HarmonyPatch(typeof(Player))]
class Player_ExecutePatch
{
  [HarmonyPatch("OnMapLoaded")]
  [HarmonyPostfix]
  public static void Postfix(bool startingOrLoadingGame, bool returningHome)
  {
    Helpers.readyForItems = true;
    Plugin.Logger.LogInfo("Map loaded");
    if (Helpers.firstMapLoad){
      Helpers.DisplayConnectionMessage();
    }
    Helpers.firstMapLoad = false;
  }
}
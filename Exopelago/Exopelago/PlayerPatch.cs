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
    Plugin.Logger.LogInfo("Map loaded");
    Helpers.firstMapLoad = false;
    Helpers.DisplayAPStory();
  }
}
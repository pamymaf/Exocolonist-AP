using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Northway.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using Exopelago.Archipelago;

namespace Exopelago;

[HarmonyPatch(typeof(MapManager))]
class Map_ExecutePatch
{
  [HarmonyPatch("LoadMap")]
  [HarmonyPostfix]
  public static void Postfix()
  {
    //Doesn't work
    Plugin.Logger.LogInfo("Map loaded");
    if (ArchipelagoClient.authenticated) {
      Story apStory = Story.FromID("apConnected");
      Result apResult = new Result();
      apStory.Execute(apResult);
    } else if (!ArchipelagoClient.authenticated) {
      Story apStory = Story.FromID("apNotConnected");
      Result apResult = new Result();
      apStory.Execute(apResult);
    }
  }
}
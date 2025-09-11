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

  }
}
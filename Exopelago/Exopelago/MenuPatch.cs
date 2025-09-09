using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Northway.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using Exopelago.Archipelago;

namespace Exopelago;

[HarmonyPatch(typeof(MainMenu))]
class MainMenu_ExecutePatch
{
  [HarmonyPatch("OnOpen")]
  [HarmonyPostfix]
  public static void Postfix(Job __instance)
  {
    if (ArchipelagoClient.hasConnected){
      Plugin.Logger.LogInfo("Main menu opened, disconnect from AP");
      ArchipelagoClient.Disconnect();
    }
  }
}
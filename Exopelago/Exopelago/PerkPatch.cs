using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Northway.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using Exopelago.Archipelago;

namespace Exopelago;

[HarmonyPatch(typeof(PerkMenu))]
class Perk_MenuPatch
{
  [HarmonyPatch("ShowPerk")]
  [HarmonyPrefix]
  public static bool Prefix(Skill skill, int perkLevel)
  {
    if (ArchipelagoClient.serverData.perksanity) {
      Plugin.Logger.LogInfo("Blocking perk popup");
      return false;
    } else {
      Plugin.Logger.LogInfo("Showing perk popup");
      return true;
    }
  }
}
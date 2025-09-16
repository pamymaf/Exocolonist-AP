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
  [HarmonyPostfix]
  public static bool Prefix(Skill skill, int perkLevel)
  {
    return false;
  }
}
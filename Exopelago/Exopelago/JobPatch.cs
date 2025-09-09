using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Northway.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using Exopelago.Archipelago;

namespace Exopelago;

[HarmonyPatch(typeof(Job))]
class Job_ExecutePatch
{
  [HarmonyPatch("Execute")]
  [HarmonyPostfix]
  public static void Postfix(Job __instance)
  {
    Plugin.Logger.LogInfo("Doing job:");
    Plugin.Logger.LogInfo($"  {__instance.jobID}");
  }
}
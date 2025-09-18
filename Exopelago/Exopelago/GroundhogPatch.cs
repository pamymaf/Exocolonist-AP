using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Northway.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Exopelago.Archipelago;

namespace Exopelago;

[HarmonyPatch(typeof(Princess))]
class Princess_SetGroundhogPatch
{
  [HarmonyPatch("SetGroundhog")]
  [HarmonyPrefix]
  public static bool Prefix(string id, string value = "true")
  {
    if (ArchipelagoClient.authenticated){
      string hogID = id.Replace("hog_", "");
      Plugin.Logger.LogInfo($"Setting hog {hogID}");
      ArchipelagoClient.SetHog(hogID, value);
      return false;
    }
    return true;
  }
}

[HarmonyPatch(typeof(Princess))]
class Princess_GetGroundhogPatch
{
  [HarmonyPatch("GetGroundhog")]
  [HarmonyPrefix]
  public static bool Prefix(string id)
  {
    if (ArchipelagoClient.authenticated){
      string hogID = id.ToLower();
      Plugin.Logger.LogInfo($"Getting hog {hogID}");
      string hog = ArchipelagoClient.GetHog(hogID);
      if (hog != null){
        return false;
      } else {
        return true;
      }
    }
    return true;
  }

  // A postfix always runs, even if prefix told the function not to
  [HarmonyPatch("GetGroundhog")]
  [HarmonyPostfix]
  public static void Postfix(ref string __result, string id)
  {
    if (ArchipelagoClient.authenticated){
      string hogID = id.Replace("hog_", "");
      string hog = ArchipelagoClient.GetHog(hogID.ToLower());
      Plugin.Logger.LogInfo($"returning hog {hog}");
      if (hog != null) {
        __result = hog;
      } else {
        __result = "";
      }
    }
  }
}

[HarmonyPatch(typeof(Princess))]
class Princess_HasGroundhogPatch
{
  [HarmonyPatch("HasGroundhog")]
  [HarmonyPrefix]
  public static bool Prefix(string id, bool evenIfDisabled = false)
  {
    if (ArchipelagoClient.authenticated){
      string hogID = id.ToLower();
      Plugin.Logger.LogInfo($"Getting hog {hogID}");
      string hog = ArchipelagoClient.GetHog(hogID);
      if (hog != null){
        return false;
      } else {
        return true;
      }
    }
    return true;
  }

  // A postfix always runs, even if prefix told the function not to
  [HarmonyPatch("HasGroundhog")]
  [HarmonyPostfix]
  public static void Postfix(ref bool __result, string id, bool evenIfDisabled = false)
  {
    if (ArchipelagoClient.authenticated){
      string hogID = id.Replace("hog_", "");
      string hog = ArchipelagoClient.GetHog(hogID);
      if (hog != null) {
        __result = bool.Parse(hog);
      } else {
        __result = false;
      }
    }
  }
}
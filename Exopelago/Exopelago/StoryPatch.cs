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

[HarmonyPatch(typeof(Story))]
class Story_ExecutePatch
{
  [HarmonyPatch("Execute")]
  [HarmonyPostfix]
  public static void Postfix(Story __instance, Result result, bool undoing = false, bool startStoryOnly = false, bool isEnding = false)
  {
    string storyID = __instance.storyID;
    Plugin.Logger.LogInfo($"{storyID} story triggered");
    switch (storyID){
      case "gamestartintro":
        JObject json = Helpers.GetConnectionInfoNewGame();
        Helpers.Connect(json);
        break;
        
      case string x when x.Contains("explorecollectible"):
        string id = __instance.storyID.Replace("explorecollectible", "");
        string apID = ItemsAndLocationsHandler.internalToAPcollectibles[id];
        Plugin.Logger.LogInfo($"Story: {storyID} ID: {id} AP ID: {apID}");
        ArchipelagoClient.ProcessLocation(apID);
        break;

      case string x when TriggerGoal(x):
        Plugin.Logger.LogInfo("Trigger goal here");
        ArchipelagoClient.SendGoal();
        break;
    }

    
  }

  [HarmonyPatch("Execute")]
  [HarmonyPostfix]
  public static bool Prefix(Story __instance, Result result, bool undoing = false, bool startStoryOnly = false, bool isEnding = false)
  {
    Plugin.Logger.LogInfo($"Prefix: {__instance.storyID}");
    string storyID = __instance.storyID;
    return true;   
  }



  public static bool TriggerGoal(string ending)
  {
    if (ending.Contains("archipelago") || ending == "ending_oldsol") {
      return false;
    } else if (ending.Contains("ending_")) {
      // This is where we detect ending type
      return true;
    } else {
      return false;
    }
  }
}

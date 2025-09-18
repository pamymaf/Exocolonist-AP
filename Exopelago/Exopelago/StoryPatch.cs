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
        Plugin.Logger.LogInfo("New game, attempting connection");
        Helpers.Connect(json);
        Helpers.ReplaceHogs();
        break;
        
      case string x when x.Contains("explorecollectible"):
        string id = __instance.storyID.Replace("explorecollectible", "");
        string apID = ItemsAndLocationsHandler.internalToAPcollectibles[id];
        Plugin.Logger.LogInfo($"Collectible {storyID} ID: {id} AP ID: {apID}");
        ArchipelagoClient.ProcessLocation(apID);
        break;

      case string x when TriggerGoal(x):
        Plugin.Logger.LogInfo("Trigger goal here");
        ArchipelagoClient.SendGoal();
        break;
    }

    
  }

  [HarmonyPatch("Execute")]
  [HarmonyPrefix]
  public static bool Prefix(Story __instance, Result result, bool undoing = false, bool startStoryOnly = false, bool isEnding = false)
  {
    string storyID = __instance.storyID;
    return true;   
  }



  public static bool TriggerGoal(string ending)
  {
    if (ending.Contains("archipelago") || ending == "ending_oldsol" || ending == "ending_end") {
      return false;
    } else if (ending.Contains("ending_")) {
      string endingName = ending.Replace("ending_", "");
      // Any ending
      if (ArchipelagoClient.serverData.ending == "any") {
        return true;
      } 
      // No hobbyist/rebel
      else if (ArchipelagoClient.serverData.ending == "no_slacker" && endingName != "hobbyist" && endingName != "rebel") {
        return true;
      }
      // Only special endings
      else if (ArchipelagoClient.serverData.ending == "special" && endingName.Contains("special")) {
        return true;
      }
      return false;
    } else {
      return false;
    }
  }
}

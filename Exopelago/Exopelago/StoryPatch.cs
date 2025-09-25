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
        ArchipelagoClient.serverData.InitializeData();
        ArchipelagoClient.RefreshUnlocks();
        ExopelagoGroundhogs.instance.Load();
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

  public static bool TriggerGoal(string ending)
  {
    if (!ending.Contains("ending")) {
      return false;
    }
    string endingName = ending.Replace("ending_", "");
    Plugin.Logger.LogInfo($"TriggerGoal: {endingName}");
    if (ending.Contains("archipelago")) {
      // AP ending
      string strNumLives = Princess.GetGroundhog("numLives");
      int intNumLives = Int32.Parse(strNumLives);
      intNumLives++;
      strNumLives = Convert.ToString(intNumLives);
      Princess.SetGroundhog("numLives", strNumLives);
      ExopelagoGroundhogs.Save();
      return false;
    } 
    else if (ending == "ending_oldsol" || ending == "ending_end") {
      return false;
    }
    else if (ArchipelagoClient.serverData.ending == "any") {
      // Any ending
      return true;
    } 
    // No hobbyist/rebel
    else if (ArchipelagoClient.serverData.ending == "no_slacker" && endingName != "hobbyist" && endingName != "rebel") {
      Plugin.Logger.LogInfo("Should goal here");
      return true;
    }
    // Only special endings
    else if (ArchipelagoClient.serverData.ending == "special" && endingName.Contains("special")) {
      return true;
    }
    return false;
  }
}

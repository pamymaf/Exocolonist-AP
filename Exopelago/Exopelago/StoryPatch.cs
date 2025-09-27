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
  [HarmonyPrefix]
  public static bool Prefix(Story __instance)
  {
    string storyID = __instance.storyID;
    if (ArchipelagoClient.serverData.buildings.ContainsKey(storyID)) {
      string destID = ArchipelagoClient.serverData.buildings[storyID];
      ExecutePatched(destID);
      return false;
    }
    return true;
  }

  [HarmonyPatch("Execute")]
  [HarmonyPostfix]
  public static void Postfix(ref Story __instance, Result result, bool undoing = false, bool startStoryOnly = false, bool isEnding = false)
  {
    string storyID = __instance.storyID;
    Plugin.Logger.LogInfo($"{storyID} story triggered");
    switch (storyID){
      case "gamestartintro":
        ArchipelagoClient.serverData.InitializeData();
        ArchipelagoClient.RefreshUnlocks();
        ExopelagoGroundhogs.instance.Load();
        Helpers.firstMapLoad = true;
        Helpers.AddSaveData();
        break;
        
      case string x when x.Contains("explorecollectible"):
        string id = __instance.storyID.Replace("explorecollectible", "");
        string apID = ItemsAndLocationsHandler.internalToAPcollectibles[id];
        Plugin.Logger.LogInfo($"Collectible {storyID} gathered. ID: {id} AP ID: {apID}");
        ArchipelagoClient.ProcessLocation(apID);
        break;

      case string x when TriggerGoal(x):
        Plugin.Logger.LogInfo("Trigger goal here");
        ArchipelagoClient.SendGoal();
        break;
    }

    
  }

  public static void ExecutePatched(string storyID)
  {
    Story story = Story.FromID(storyID);
    Result result = new ();

    Console.WriteLine($"Executing other story {storyID}");
    story.Reset();
    result.story = story;
    Princess.SetResult(result);
    Savegame.instance.Autosave(1);
    result.SetDefaultImages();

    story.entryChoice.Execute(result);
    Singleton<PortraitMenu>.instance.UpdatePortraitAndSkills();
    Singleton<ResultsMenu>.instance.ShowResult();
  }

  public static bool TriggerGoal(string ending)
  {
    if (!ending.Contains("ending")) {
      return false;
    }
    string endingName = ending.Replace("ending_", "");
    Plugin.Logger.LogInfo($"TriggerGoal scanning: {endingName}");
    // AP ending
    if (ending.Contains("archipelago")) {
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
    // Any ending
    else if (ArchipelagoClient.serverData.ending == "any") {
      Plugin.Logger.LogInfo("Should goal here");
      return true;
    } 
    // No hobbyist/rebel
    else if (ArchipelagoClient.serverData.ending == "no_slacker" && endingName != "hobbyist" && endingName != "rebel") {
      Plugin.Logger.LogInfo("Should goal here");
      return true;
    }
    // Only special endings
    else if (ArchipelagoClient.serverData.ending == "special" && endingName.Contains("special")) {
      Plugin.Logger.LogInfo("Should goal here");
      return true;
    }
    return false;
  }
}

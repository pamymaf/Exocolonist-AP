using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Northway.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using Exopelago.Archipelago;

namespace Exopelago;

[HarmonyPatch(typeof(Story))]
class Story_ExecutePatch
{
  [HarmonyPatch("Execute")]
  [HarmonyPostfix]
  public static void Postfix(Story __instance, Result result, bool undoing = false, bool startStoryOnly = false, bool isEnding = false)
  {
    Plugin.Logger.LogInfo($"{__instance.storyID} story triggered");
    if (__instance.storyID == "gamestartintro") {
      Plugin.Logger.LogInfo("Connection here");
      ArchipelagoClient.Connect("localhost", 38281, "Player1", null);
      // TODO: Add ap info to main menu. Or at least a file with credentials
      // TODO: Indicate to the user they're connected
    } else if (__instance.storyID == "visited_colonystrato") {
      //For future use, tells the client that the game is fully loaded
      //ArchipelagoClient.readyForItems = true; 
    } else if (__instance.storyID.Contains("explorecollectible")) {
      string id = __instance.storyID.Replace("explorecollectible", "");
      string apID = ItemsAndLocationsHandler.internalToAPcollectibles[id];
      Plugin.Logger.LogInfo($"Story: {__instance.storyID} ID: {id} AP ID: {apID}");
      ArchipelagoClient.ProcessItemSent(apID);
    }
  }

  [HarmonyPatch("Execute")]
  [HarmonyPostfix]
  public static bool Prefix(Story __instance, Result result, bool undoing = false, bool startStoryOnly = false, bool isEnding = false)
  {
    Plugin.Logger.LogInfo($"Prefix: {__instance.storyID}");
  /*  TODO: Building unlocks. This code will spam the logs and lock the mouse as written
    if (ItemsAndLocationsHandler.internalToAPBuildings.ContainsKey(__instance.storyID)) {
      if (ArchipelagoClient.serverData.receivedBuildings.Contains(__instance.storyID)) {
        return true;
      } else {
        return false;
      }
    }
    */
    // TODO: Building rando
    return true;
  }

  public static void GiveCollectible(string collectible) 
  {
    // Gives collectible without popup
    Plugin.Logger.LogInfo($"Attempting to give {collectible}");
    CardData cardData = CardData.FromID(collectible);
    PrincessCards.AddCard(cardData);
    Princess.SetMemory("mem_foundCollectible");
  }

  public static void EndGame()
  {
    // Force ending screen
    Result result = new Result();
    Story story = Story.FromID("ending_end");
    story.Execute(result);
  }
}

[HarmonyPatch(typeof(StoryCall))]
class StoryCall_ExecutePatch
{
  [HarmonyPatch("endgame")]
  [HarmonyPostfix]
  public static void Prefix(Story __instance, Result result, bool undoing = false, bool startStoryOnly = false, bool isEnding = false)
  {
    // Victory logic here?
  }
}
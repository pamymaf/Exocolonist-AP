using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Northway.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.IO;
using Exopelago.Archipelago;

namespace Exopelago;

public static class Helpers
{
  //Not used yet
  public static void AddSkillPoints(string skillID, int value)
  {
    Skill skill = Skill.FromID(skillID);
    if (skill == null)
    {
      Plugin.Logger.LogError($"Could not find skill with ID: {skillID}");
      return;
    }

    int currentValue = Princess.GetSkill(skillID, includeGear: false);
    int newValue = currentValue + value;
    Princess.SetSkill(skillID, newValue, null);

    Plugin.Logger.LogInfo($"Added {value} to {skillID}. Old: {currentValue}. New: {newValue}");
  }

  public static JObject GetConnectionInfo()
  {
    JObject json = JObject.Parse(File.ReadAllText("connectionInfo.json"));
    return json;
  }
  
  // We do this so we can intercept SetMemory and use a special prefix to detect it's AP unlocking it, not the game
  public static void UnlockJob(string name)
  {
    Plugin.Logger.LogInfo($"Attempting to unlock {name}");
    Princess.SetMemory($"unlockjob_{name}");
  }

  // Called from SetMemory prefix, needs to return a bool on if the memory should be set
  public static bool ProcessMemory(string id)
  {
    switch (id) {
      case string x when x.StartsWith("unlockjob_"):
        string receivedJob = id.RemoveStart("unlockjob_");
        if (ArchipelagoClient.serverData.receivedJobs.Contains(receivedJob)){
          if (!Princess.cards.Contains(receivedJob)){
            GiveCard(receivedJob);
          }
          Princess.AddMemory($"job_{receivedJob}", "true");
          Plugin.Logger.LogInfo($"{receivedJob} unlocked.");
          return false;
        } else {
          return false;
        }

      case string x when x.StartsWith("job_"):
        string sentJob = id.RemoveStart("job_");
        string apJobName = ItemsAndLocationsHandler.internalToAPJobs[sentJob];
        Plugin.Logger.LogInfo($"Trying to send AP location {apJobName}");
        ArchipelagoClient.ProcessLocation(apJobName);
        return false;

      case string x when ItemsAndLocationsHandler.storyEvents.ContainsKey(id):
        string locationName = ItemsAndLocationsHandler.storyEvents[id];
        Plugin.Logger.LogInfo($"Trying to send AP location {locationName} with id {id}");
        ArchipelagoClient.ProcessLocation(locationName);
        return true;

      default:
        return true;
    }
  }

  public static void GiveCard(string collectible) 
  {
    // Gives collectible without popup
    Plugin.Logger.LogInfo($"Attempting to give {collectible}");
    CardData cardData = CardData.FromID(collectible);
    PrincessCards.AddCard(cardData);
    Princess.SetMemory("mem_foundCollectible");
  }

  public static void AddSaveData(Dictionary<string, string> data)
  {
    Princess.SetMemory(data["apServer"]);
    Princess.SetMemory(data["apPort"]);
    Princess.SetMemory(data["apSlot"]);
    Princess.SetMemory(data["apPass"]);
    Princess.SetMemory(data["apSeed"]);
  }

  public static int GetAge()
  {
    return Princess.age;
  }
}
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Northway.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Globalization;
using Exopelago.Archipelago;

namespace Exopelago;


public class Helpers
{
  public static bool readyForItems = false;
  public static bool firstMapLoad = true;


  // ========== Connection ========== \\
  // Get connection info
  public static JObject GetConnectionInfoSaveGame()
  {
    try {
      JObject json = new JObject();
      json["ip"] = Princess.GetMemory("apServer");
      json["port"] = Princess.GetMemory("apPort");
      json["slot"] = Princess.GetMemory("apSlot");
      json["pass"] = Princess.GetMemory("apPass");
      json["seed"] = Princess.GetMemory("apSeed");
      return json;
    } catch {
      return null;
    }
  }

  // Set connection info
  public static void AddSaveData()
  {
    Princess.SetMemory("apServer", ArchipelagoClient.serverData.uri);
    Princess.SetMemory("apPort", ArchipelagoClient.serverData.port);
    Princess.SetMemory("apSlot", ArchipelagoClient.serverData.slotName);
    Princess.SetMemory("apPass", ArchipelagoClient.serverData.password);
    Princess.SetMemory("apSeed", ArchipelagoClient.serverData.seed);
  }

  

  // ========== Unlockers ========== \\
  // We do this so we can intercept AddMemory and use a special prefix to detect it's AP unlocking it, not the game
  public static void UnlockJob(string name)
  {
    Plugin.Logger.LogInfo($"Attempting to unlock job {name}");
    Princess.SetMemory($"unlockjob_{name}");
  }


  // Gives collectible without popup
  // Used for collectibles and jobs as gear cards
  public static void GiveCard(string collectible) 
  {
    Plugin.Logger.LogInfo($"Attempting to give {collectible}");
    CardData cardData = CardData.FromID(collectible);
    PrincessCards.AddCard(cardData);
    Princess.SetMemory("mem_foundCollectible");
  }

  // For some reason Perk unlocks don't go through SetMemory but instead AddMemory
  public static void UnlockPerk(string skill)
  {
    Plugin.Logger.LogInfo($"UnlockPerk {skill}");
    int maxPerk = ArchipelagoClient.serverData.receivedPerk[skill];
    Plugin.Logger.LogInfo($"maxPerk {maxPerk}");
    if (maxPerk == 1) {
      Princess.AddMemory($"unlockskillperk_{skill}1");
    } else if (maxPerk == 2) {
      Princess.AddMemory($"unlockskillperk_{skill}2");
    } else if (maxPerk == 3) {
      Princess.AddMemory($"unlockskillperk_{skill}3");
    }
  }


  // ========== Groundhogs ========== \\
  // TODO LOCAL SAVES
  public static void ReplaceHogs() {
    var hogs = ArchipelagoClient.GetAllHogs();
    Plugin.Logger.LogInfo($"Server hogs: {hogs}");
    Plugin.Logger.LogInfo($"Old hogs: {PrettyDict(Groundhogs.instance.groundhogs)}");
    Groundhogs.instance.groundhogs = new StringDictionary();
    foreach (var hog in hogs) {
      Groundhogs.instance.groundhogs.Set(hog.Key, hog.Value);
    }
  }


  // ========== AP messages ========== \\
  // Show AP hints
  public static void DisplayAPHint(string sender, string receiver, string item, string location) {
    if (readyForItems){
      string slotName = ArchipelagoClient.serverData.slotName;
      if (sender == slotName && receiver == slotName) {
        PlayerText.Show($"Your {item} is at {location} in your world");
      } else if (sender == slotName) {
        PlayerText.Show($"{receiver}'s {item} is at {location} in your world");
      } else if (receiver == slotName) {
        PlayerText.Show($"Your {item} is at {location} in {sender}'s world");
      }
    }
  }

  // Show AP send/receive
  public static void DisplayAPItem(string sender, string receiver, string item) {
    if (readyForItems){
      string slotName = ArchipelagoClient.serverData.slotName;
      if (sender == slotName && receiver == slotName) {
        PlayerText.Show($"You sent yourself {item}");
      } else if (sender == slotName) {
        PlayerText.Show($"You sent {receiver} {item}");
      } else if (receiver == slotName) {
        PlayerText.Show($"{sender} sent you {item}");
      }
    }
  }

  // Show AP connected
  public static void DisplayConnectionMessage(string message = null) {
    if (readyForItems){
      if (message != null) {
        PlayerText.Show(message);
      } else if (ArchipelagoClient.authenticated){
        PlayerText.Show("Archipelago connected");
      } else {
        PlayerText.Show("Archipelago not connected");
      }
    }
  }


  // Add skill points
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

  // Add love points



  


  // ========== Bulk processing ========== \\
  // Called from AddMemory prefix, needs to return a bool on if the memory should be set
  public static bool ProcessMemory(string id)
  {
    if (!ArchipelagoClient.authenticated) {
      return true;
    }

    string perkID;
    switch (id) {
      case string x when x.StartsWith("skillperk_") && ArchipelagoClient.serverData.perksanity:
        perkID = id.Replace("skillperk_", "");
        string perk = perkID.Insert(perkID.Length - 1, " Perk ");
        string location = CultureInfo.CurrentCulture.TextInfo.ToTitleCase($"{perk}".ToLower());
        Plugin.Logger.LogInfo($"AddMemory location {id}");
        ArchipelagoClient.ProcessLocation(location);
        return false;

      case string x when x.StartsWith("unlockskillperk_"):
        perkID = id.Replace("unlock", "");
        Princess.memories.AddSafe(perkID, "true");
        Plugin.Logger.LogInfo($"AddMemory AddSafe {perkID}");
        Plugin.Logger.LogInfo($"Trying to unlock {perkID}");
        Perk.UpdateCurrentPerks();
        return false;

      case string x when x.StartsWith("unlockjob_"):
        string receivedJob = id.RemoveStart("unlockjob_");
        if (!Princess.cards.Contains(receivedJob.ToLower())){
          GiveCard(receivedJob);
        }
        Princess.memories.AddSafe($"job_{receivedJob}", "true");
        Plugin.Logger.LogInfo($"{receivedJob} unlocked");
        return false;

      case string x when x.StartsWith("job_"):
        return false;

      case string x when x.StartsWith("sendjob_"):
        string sendjob = id.RemoveStart("sendjob_");
        string apJobName = ItemsAndLocationsHandler.internalToAPJobs[sendjob];
        Plugin.Logger.LogInfo($"Trying to send AP location {apJobName}");
        ArchipelagoClient.ProcessLocation(apJobName);
        return true;

      case string x when ItemsAndLocationsHandler.storyEvents.ContainsKey(id):
        string locationName = ItemsAndLocationsHandler.storyEvents[id];
        Plugin.Logger.LogInfo($"Trying to send AP location {locationName} with id {id}");
        ArchipelagoClient.ProcessLocation(locationName);
        return true;

      case "couldsurvey":
        string specialLocation = ItemsAndLocationsHandler.internalToAPJobs["explorenearby"];
        Plugin.Logger.LogInfo($"Trying to send AP location {specialLocation} with id {id}");
        ArchipelagoClient.ProcessLocation(specialLocation);
        return true;

      default:
        return true;
    }
  }


  // ========== Misc ========== \\
  public static string PrettyDict(Dictionary<string, string> input) {
    string pretty = input.Aggregate(
      "{", 
      (str, kv) => str += $"\"{kv.Key}\": \"{kv.Value}\", ", 
      (str) => str += "}"
    );
    return pretty;
  }

  // So that archipelagoData isn't talking to Princess directly
  public static int GetAge()
  {
    return Princess.age;
  }

}
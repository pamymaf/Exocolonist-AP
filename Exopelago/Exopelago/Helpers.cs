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
using Exopelago.Archipelago;

namespace Exopelago;

public class Helpers
{
  public static bool readyForItems = false;
  public static bool firstMapLoad = true;

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

  public static JObject GetConnectionInfoNewGame()
  {
    JObject json = JObject.Parse(File.ReadAllText("connectionInfo.json"));
    return json;
  }

  public static JObject GetConnectionInfoSaveGame()
  {
    JObject json = new JObject();
    json["ip"] = Princess.GetMemory("apServer");
    json["port"] = Princess.GetMemory("apPort");
    json["slot"] = Princess.GetMemory("apSlot");
    json["pass"] = Princess.GetMemory("apPass");
    json["seed"] = Princess.GetMemory("apSeed");
    return json;
  }
  
  public static void Connect(JObject json) {
    Plugin.Logger.LogInfo("Connection here");
    Plugin.Logger.LogInfo(json.ToString(Formatting.None));
    ArchipelagoClient.Connect((string)json["ip"], (string)json["port"], (string)json["slot"], (string)json["pass"]);
    // TODO: Add ap info to main menu
    Dictionary<string, string> apData = new () {
      {"apServer", (string)json["ip"]},
      {"apPort", (string)json["port"]},
      {"apSlot", (string)json["slot"]},
      {"apPass", (string)json["pass"]},
      {"apSeed", ArchipelagoClient.session.RoomState.Seed},
    };
    Helpers.AddSaveData(apData);
    Helpers.firstMapLoad = true;
}


  // We do this so we can intercept SetMemory and use a special prefix to detect it's AP unlocking it, not the game
  public static void UnlockJob(string name)
  {
    Plugin.Logger.LogInfo($"Attempting to unlock job {name}");
    Princess.SetMemory($"unlockjob_{name}");
  }

  // Called from SetMemory prefix, needs to return a bool on if the memory should be set
  public static bool ProcessMemory(string id)
  {
    if (!ArchipelagoClient.authenticated) {
      return true;
    }
    switch (id) {
      case string x when x.StartsWith("unlockjob_"):
        string receivedJob = id.RemoveStart("unlockjob_");
        if (!Princess.cards.Contains(receivedJob.ToLower())){
          GiveCard(receivedJob);
        }
        Princess.AddMemory($"job_{receivedJob}", "true");
        Plugin.Logger.LogInfo($"{receivedJob} unlocked.");
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
    Princess.SetMemory("apServer", data["apServer"]);
    Princess.SetMemory("apPort", data["apPort"]);
    Princess.SetMemory("apSlot", data["apSlot"]);
    Princess.SetMemory("apPass", data["apPass"]);
    Princess.SetMemory("apSeed", data["apSeed"]);
  }

  public static int GetAge()
  {
    return Princess.age;
  }


  public static void DisplayConnectionMessage() {
    if (readyForItems){
      if (ArchipelagoClient.authenticated){
        PlayerText.Show("AP connected");
      } else {
        PlayerText.Show("AP not connected");
      }
    }
  }


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


  public static void ReplaceHogs() {
    var hogs = ArchipelagoClient.GetAllHogs();

    string pretty = Groundhogs.instance.groundhogs.Aggregate(
      "{", 
      (str, kv) => str += $"\"{kv.Key}\": \"{kv.Value}\", ", 
      (str) => str += "}"
    );
    Plugin.Logger.LogInfo($"ReplaceHogs: {pretty}");


    Groundhogs.instance.groundhogs = new StringDictionary();


    pretty = Groundhogs.instance.groundhogs.Aggregate(
      "{", 
      (str, kv) => str += $"\"{kv.Key}\": \"{kv.Value}\", ", 
      (str) => str += "}"
    );
    Plugin.Logger.LogInfo($"ReplaceHogs: {pretty}");


    Plugin.Logger.LogInfo("Replacing all hogs");
    foreach (var hog in hogs) {
      Groundhogs.instance.groundhogs.Set(hog.Key, hog.Value);
    }

    pretty = Groundhogs.instance.groundhogs.Aggregate(
      "{", 
      (str, kv) => str += $"\"{kv.Key}\": \"{kv.Value}\", ", 
      (str) => str += "}"
    );
    Plugin.Logger.LogInfo($"ReplaceHogs: {pretty}");
  }
}
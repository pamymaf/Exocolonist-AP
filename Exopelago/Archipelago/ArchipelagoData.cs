using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Linq;
using Archipelago.MultiClient.Net.Models;
using Newtonsoft.Json;
using UnityEngine;
using Exopelago;

namespace Exopelago.Archipelago;

public class ArchipelagoData
{
  public string uri = "archipelago.gg";
  public string port = "38281";
  public string slotName = "Player1";
  public string password = "";
  public string seed;
  public int index;
  public Dictionary<string, object> slotData;
  public static bool deathLink = false;
  public List<long> checkedLocations;
  public Dictionary<long, int> receivedItems;
  public Dictionary<long, Dictionary<string, List<long>>> locationData;

  public bool friendsanity;
  public bool datesanity;
  public string ending;
  public bool perksanity;

  public List<string> unlockedJobs;
  public List<string> receivedJobs;
  public Dictionary<string, string> groundhogs;
  public Dictionary<string, int> receivedFriendship;
  public List<string> receivedBuildings;
  public Dictionary<string, int> receivedPerk;
  public Dictionary<string, string> buildings = new () {
    {"garrison", "garrison"},
    {"engineering", "engineering"},
    {"quarters", "quarters"},
    {"command", "command"},
    {"geoponics", "geoponics"},
    {"expeditions", "expeditions"},
  };
  public int maxAge = 10;


  public void InitializeData()
  {
    seed = ArchipelagoClient.session.RoomState.Seed;

    ExopelagoGroundhogs.isLoaded = false;
    ExopelagoGroundhogs.filename = $"{slotName}-{seed}.json";

    Groundhogs.instance.groundhogs = new StringDictionary ();
    ExopelagoGroundhogs.instance.groundhogs = new StringDictionary();

    ExopelagoGroundhogs.isLoaded = true;
    ExopelagoGroundhogs.instance.Load();

    index = ArchipelagoClient.offlineReceivedItems;
    receivedItems = new Dictionary<long, int>();
    unlockedJobs = new ();
    receivedJobs = new ();
    groundhogs = new ();
    receivedFriendship = new ();
    maxAge = 10;
    var slotData = ArchipelagoClient.session.DataStorage.GetSlotData();
    if (Convert.ToString(slotData["friendsanity"]) == "1") {
      friendsanity = true;
    }
    if (Convert.ToString(slotData["datesanity"]) == "1") {
      datesanity = true;
      ItemsAndLocationsHandler.storyEvents = new (
        ItemsAndLocationsHandler.nonDateEvents.Concat(ItemsAndLocationsHandler.dateEvents).ToDictionary(x=>x.Key, x=>x.Value)
      );
    } else {
      ItemsAndLocationsHandler.storyEvents = new (
        ItemsAndLocationsHandler.nonDateEvents
      );
    }
    switch (Convert.ToInt32(slotData["ending"])) {
      case 1:
        ending = "no_slacker";
        break;
      
      case 2:
        ending = "special";
        break;
      
      default:
        ending = "any";
        break;
    }
    if (Convert.ToString(slotData["perksanity"]) == "1") {
      perksanity = true;
      receivedPerk = new () {
        {"empathy", 0},
        {"creativity", 0},
        {"persuasion", 0},
        {"bravery", 0},
        {"reasoning", 0},
        {"engineering", 0},
        {"organization", 0},
        {"biology", 0},
        {"toughness", 0},
        {"combat", 0},
        {"perception", 0},
        {"animals", 0},
      };
    }
    if (Convert.ToString(slotData["building_rando"]) == "1") {
      buildings = RandomizeBuildings();
    }


    var pretty = slotData.Aggregate(
      "{", 
      (str, kv) => str += $"\"{kv.Key}\": \"{kv.Value}\", ", 
      (str) => str += "}"
    );

    Plugin.Logger.LogInfo($"Settings: {pretty}");

    pretty = buildings.Aggregate(
      "{", 
      (str, kv) => str += $"\"{kv.Key}\": \"{kv.Value}\", ", 
      (str) => str += "}"
    );

    Plugin.Logger.LogInfo($"Buildings: {pretty}");
  }

  public void RaiseAge()
  {
    int currentAge = Exopelago.Helpers.GetAge();
    int newMaxAge = maxAge + 1;
    maxAge = newMaxAge;
  }

  public Dictionary<string, string> RandomizeBuildings()
  {
    int hash;
    hash = seed.GetHashCode() ^ slotName.GetHashCode();
    System.Random rng = new System.Random(hash);
    List<string> buildingList = new () {
      "garrison",
      "engineering",
      "quarters",
      "command",
      "geoponics",
      "expeditions",
    };
    int n = buildingList.Count;

    while (n > 1) {
      n--;
      int k = rng.Next(n + 1);
      string value = buildingList[k];
      buildingList[k] = buildingList[n];
      buildingList[n] = value;
    }
    Dictionary<string, string> buildingDict = new ();
    for (int i = 0; i < buildingList.Count; i++) {
      string key = buildings.ElementAt(i).Key;
      buildingDict[key] = buildingList[i];
    }

    return buildingDict;
  }
}
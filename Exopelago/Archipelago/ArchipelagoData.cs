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
  public int port = 38281;
  public string slotName = "";
  public string password = "";
  public int index;
  public string seedName = "Unknown";
  public Dictionary<string, object> slotData;
  public static bool deathLink = false;
  public List<long> checkedLocations;
  public Dictionary<long, int> receivedItems;
  public Dictionary<long, Dictionary<string, List<long>>> locationData;

  public bool friendsanity;
  public bool datesanity;
  public string ending;

  public List<string> unlockedJobs;
  public List<string> receivedJobs;
  public Dictionary<string, string> groundhogs;
  public Dictionary<string, int> receivedFriendship;
  public List<string> receivedBuildings;
  public int maxAge = 10;

  public void StartNewSeed()
  {
    Console.WriteLine("Creating new seed data");
    index = ArchipelagoClient.offlineReceivedItems;
    receivedItems = new Dictionary<long, int>();
    unlockedJobs = new ();
    receivedJobs = new ();
    groundhogs = new ();
    receivedFriendship = new ();
    maxAge = 10;
    var slotData = ArchipelagoClient.session.DataStorage.GetSlotData();
    if (Convert.ToInt32(slotData["friendsanity"]) == 1) {
      friendsanity = true;
    }
    if (Convert.ToInt32(slotData["datesanity"]) == 1) {
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



    var pretty = slotData.Aggregate(
      "{", 
      (str, kv) => str += $"\"{kv.Key}\": \"{kv.Value}\", ", 
      (str) => str += "}"
    );

    Plugin.Logger.LogInfo(pretty);
  }

  public void RaiseAge()
  {
    int currentAge = Exopelago.Helpers.GetAge();
    Plugin.Logger.LogInfo($"Current max age: {currentAge}");
    int newMaxAge = maxAge + 1;
    maxAge = newMaxAge;
  }

}
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Archipelago.MultiClient.Net.Models;
using Newtonsoft.Json;
using UnityEngine;
using Exopelago;

namespace Exopelago.Archipelago;

public class ArchipelagoData
{
  public string Uri = "archipelago.gg";
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
  }

  public void RaiseAge()
  {
    int currentAge = Exopelago.Princess_PrincessMonthPatch.GetAge();
    Plugin.Logger.LogInfo($"Current max age: {currentAge}");
    int newMaxAge = maxAge + 1;
    maxAge = newMaxAge;
  }

}
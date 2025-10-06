using System;
using System.Collections.Generic;
using System.Linq;

namespace Exopelago.Archipelago;

public class ArchipelagoData
{
  public string uri = "archipelago.gg";
  public string port = "38281";
  public string slotName = "Player1";
  public string password = "";
  public string seed;
  public int index;
  public Dictionary<string, object> slotData; // Slot options
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
  public Dictionary<string, int> receivedFriendship; // Not used yet
  public List<string> receivedBuildings; // Not used yet
  public Dictionary<string, int> receivedPerk;
  public Dictionary<string, string> buildings = new () {
    {"garrison", "garrison"},
    {"engineering", "engineering"},
    {"quarters", "quarters"},
    {"command", "command"},
    {"geoponics", "geoponics"},
    {"expeditions", "expeditions"},
  };
  public Dictionary<string, string> stratosCharacters = new () {
    {"tang", "tang"},
    {"tammy", "tammy"},
    {"cal", "cal"},
    {"dys", "dys"},
    {"anemone", "anemone"},
    {"mom", "mom"},
    {"dad", "dad"},
    {"marz", "marz"},
  };
  public Dictionary<string, string> allCharacters = new() {
    {"tang", "tang"},
    {"tammy", "tammy"},
    {"cal", "cal"},
    {"dys", "dys"},
    {"anemone", "anemone"},
    {"mom", "mom"},
    {"dad", "dad"},
    {"marz", "marz"},
    {"vace", "vace"},
    {"rex", "rex"},
    {"nomi", "nomi"},
  };
  public bool forceBattles = false;
  public int maxAge = 10;
  public bool character_rando;
  public bool building_rando;

  public static string GroundhogsFileNameBase {
    set;
    private get;
  } = "Groundhogs";
  public static string GroundhogsFileName {
    get {
      return $"{GroundhogsFileNameBase}.json";
    }
  }
  public static string GroundhogsFileNameBackup {
    get {
      return $"{GroundhogsFileNameBase}.bak";
    }
  }
  public static string GroundhogsFileNameOld {
    get {
      return $"{GroundhogsFileNameBase}_old.json";
    }
  }


  public void InitializeData()
  {
    seed = ArchipelagoClient.session.RoomState.Seed;
    index = ArchipelagoClient.offlineReceivedItems; // Need to use this for refreshunlocks consumables
    receivedItems = new Dictionary<long, int>();
    unlockedJobs = new ();
    receivedJobs = new ();
    receivedFriendship = new (); // Not used yet
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
      building_rando = true;
      buildings = RandomizeBuildings();
    }
    if (Convert.ToString(slotData["character_rando"]) == "1") {
      character_rando = true;
      stratosCharacters = RandomizeCharacters(stratosCharacters);
      allCharacters = RandomizeCharacters(allCharacters);
    }

    if (slotData.ContainsKey("force_battles")) { // Remove after no release async is done
      if (Convert.ToString(slotData["force_battles"]) == "1") {
        forceBattles = true;
      } else {
        forceBattles = false;
      }
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
    maxAge++;
  }

  public Dictionary<string, string> RandomizeBuildings()
  {
    int hash;
    hash = Tuple.Create(seed, slotName).GetHashCode();
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

    while (n > 1) { // TODO: Make this a for loop
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

  public Dictionary<string, string> RandomizeCharacters(Dictionary<string, string> characters)
  {
    int hash;
    hash = Tuple.Create(seed, slotName).GetHashCode();
    System.Random rng = new System.Random(hash);
    List<string> charaList = new (characters.Keys);
    int n = charaList.Count;

    while (n > 1) { // TODO: Make this a for loop
      n--;
      int k = rng.Next(n + 1);
      string value = charaList[k];
      charaList[k] = charaList[n];
      charaList[n] = value;
    }
    Dictionary<string, string> charaDict = new ();
    for (int i = 0; i < charaList.Count; i++) {
      string key = characters.ElementAt(i).Key;
      charaDict[key] = charaList[i];
    }

    return charaDict;
  }
}
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
      if (ArchipelagoClient.authenticated) {
        return $"{GroundhogsFileNameBase}.json";
      } else {
        return "Groundhogs.json";
      }
    }
  }
  public static string GroundhogsFileNameBackup {
    get {
      if (ArchipelagoClient.authenticated) {
        return $"{GroundhogsFileNameBase}.bak";
      } else {
        return "Groundhogs.bak";
      }
    }
  }
  public static string GroundhogsFileNameOld {
    get {
      if (ArchipelagoClient.authenticated) {
        return $"{GroundhogsFileNameBase}_old.json";
      } else {
        return "Groundhogs_old.json";
      }
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
    
    object keyChecker;
    var slotData = ArchipelagoClient.session.DataStorage.GetSlotData();
    if (slotData.TryGetValue("friendsanity", out keyChecker) && Convert.ToString(slotData["friendsanity"]) == "1") {
      friendsanity = true;
    }
    if (slotData.TryGetValue("datesanity", out keyChecker) && Convert.ToString(slotData["datesanity"]) == "1") {
      datesanity = true;
      ItemsAndLocationsHandler.storyEvents = new (
        ItemsAndLocationsHandler.nonDateEvents.Concat(ItemsAndLocationsHandler.dateEvents).ToDictionary(x=>x.Key, x=>x.Value)
      );
    } else {
      ItemsAndLocationsHandler.storyEvents = new (
        ItemsAndLocationsHandler.nonDateEvents
      );
    }

    if (slotData.TryGetValue("ending", out keyChecker)) {
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
    }

    if (slotData.TryGetValue("perksanity", out keyChecker) && Convert.ToString(slotData["perksanity"]) == "1") {
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

    int hash = Tuple.Create(seed, slotName).GetHashCode();
    if (slotData.TryGetValue("building_rando", out keyChecker) && Convert.ToString(slotData["building_rando"]) == "1") {
      building_rando = true;
      buildings = Helpers.RandomizeDict(buildings, hash);
    }
    if (slotData.TryGetValue("character_rando", out keyChecker) && Convert.ToString(slotData["character_rando"]) == "1") {
      character_rando = true;
      stratosCharacters = Helpers.RandomizeDict(stratosCharacters, hash);
      allCharacters = Helpers.RandomizeDict(allCharacters, hash);
    }

    if (slotData.TryGetValue("force_battles", out keyChecker) && Convert.ToString(slotData["force_battles"]) == "1") {
      forceBattles = true;
    } else {
      forceBattles = false;
    }

    Plugin.Logger.LogInfo($"Settings: {Helpers.PrettyDict(slotData)}");
    Plugin.Logger.LogInfo($"Buildings: {Helpers.PrettyDict(buildings)}");
  }

  public void RaiseAge()
  {
    maxAge++;
  }
}
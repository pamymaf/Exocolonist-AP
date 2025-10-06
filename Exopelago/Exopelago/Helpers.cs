using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Globalization;
using Archipelago.MultiClient.Net.MessageLog.Messages;
using Exopelago.Archipelago;

namespace Exopelago;


public static class Helpers
{
  public static bool readyForItems = false;
  public static bool firstMapLoad = true;


  // ========== Connection ========== \\
  // Get connection info
  public static JObject GetConnectionInfoSaveGame()
  {
    // If keys don't exist, the json entry will be empty. Ex: (string)json["apport"] = ""
    JObject json = new JObject();
    json["apserver"] = Princess.GetMemory("apServer");
    json["apport"] = Princess.GetMemory("apPort");
    json["apslot"] = Princess.GetMemory("apSlot");
    json["appass"] = Princess.GetMemory("apPass");
    json["apseed"] = Princess.GetMemory("apSeed");
    return json;
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
    Princess.AddMemory($"unlockjob_{name.ToLower()}");
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


  // ========== AP messages ========== \\
  public static void DisplayAPMessage(string message)
  {
    if (readyForItems){
      PlayerText.Show(message);
    }
  }

  public static void DisplayAPMessage(HintItemSendLogMessage message)
  {
    var receiver = message.Receiver.Name;
    var sender = message.Sender.Name;
    var networkItem = message.Item;
    var item = networkItem.ItemName;
    var location = networkItem.LocationDisplayName;
    string slotName = ArchipelagoClient.serverData.slotName;
    if (sender == slotName && receiver == slotName) {
      DisplayAPMessage($"Your {item} is at {location} in your world");
    } else if (sender == slotName) {
      DisplayAPMessage($"{receiver}'s {item} is at {location} in your world");
    } else if (receiver == slotName) {
      DisplayAPMessage($"Your {item} is at {location} in {sender}'s world");
    }
  }

  public static void DisplayAPMessage(ItemSendLogMessage message)
  {
    if (message is HintItemSendLogMessage hintMessage) {
      DisplayAPMessage(hintMessage);
      return;
    }
    var receiver = message.Receiver.Name;
    var sender = message.Sender.Name;
    var networkItem = message.Item;
    var item = networkItem.ItemName;
    string slotName = ArchipelagoClient.serverData.slotName;
    if (sender == slotName && receiver == slotName) {
      DisplayAPMessage($"You sent yourself {item}");
    } else if (sender == slotName) {
      DisplayAPMessage($"You sent {receiver} {item}");
    } else if (receiver == slotName) {
      DisplayAPMessage($"{sender} sent you {item}");
    }
  }
  
  public static void DisplayAPMessage()
  {
    if (ArchipelagoClient.authenticated){
      DisplayAPMessage("Archipelago connected");
    } else {
      DisplayAPMessage("Archipelago not connected");
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

  // TODO Add love points

  // ========== Bulk processing ========== \\
  // Called from AddMemory prefix, needs to return a bool on if the memory should be set
  public static bool ProcessMemory(string id, object value)
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
        Plugin.Logger.LogInfo($"Trying to send AP location {location}");
        ArchipelagoClient.ProcessLocation(location);
        return false;

      case string x when x.StartsWith("unlockskillperk_"):
        perkID = id.Replace("unlock", "");
        Princess.memories.AddSafe(perkID, "true");
        Plugin.Logger.LogInfo($"{perkID} unlocked");
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
      
      case "leader":
        if (value as string == "marz") {
          ArchipelagoClient.ProcessLocation("Marz Governor");
        } else if (value as string == "player") {
          ArchipelagoClient.ProcessLocation("Become Governor");
        }
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
  public static string PrettyDict<T,U>(Dictionary<T, U> input) {
    string pretty = input.Aggregate(
      "{", 
      (str, kv) => str += $"\"{kv.Key.ToString()}\": \"{kv.Value.ToString()}\", ", 
      (str) => str += "}"
    );
    return pretty;
  }




  public static Dictionary<string, string> RandomizeDict(Dictionary<string, string> refDict, int seed)
  {
    System.Random rng = new System.Random(seed);
    List<string> randoList = new (refDict.Keys);
    int n = randoList.Count;

    while (n > 1) { // TODO: Make this a for loop
      n--;
      int k = rng.Next(n + 1);
      string value = randoList[k];
      randoList[k] = randoList[n];
      randoList[n] = value;
    }
    Dictionary<string, string> randoDict = new ();
    for (int i = 0; i < randoList.Count; i++) {
      string key = refDict.ElementAt(i).Key;
      randoDict[key] = randoList[i];
    }

    return randoDict;
  }

}
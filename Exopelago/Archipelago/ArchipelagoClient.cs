using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Newtonsoft.Json.Linq;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Exceptions;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.MessageLog.Messages;
using Archipelago.MultiClient.Net.Packets;
using Archipelago.MultiClient.Net.Models;
using Exopelago;

namespace Exopelago.Archipelago;
public class ArchipelagoClient
{
  public const string GameName = "Exocolonist";
  public const string ModName = "Exopelago";
  public const string ModVersion = "0.1.0";

  public static bool authenticated;
  private static bool attemptingConnection;
  public static bool offline;
  public static ArchipelagoData serverData = new ();
  public static int offlineReceivedItems;
  public static ArchipelagoSession session;

  public static bool readyForItems = false;


  public static void RefreshUnlocks() {
    foreach (ItemInfo item in session.Items.AllItemsReceived) {
      if (!ItemsAndLocationsHandler.apToInternalCollectibles.ContainsKey(item.ItemName)) {
        Plugin.Logger.LogInfo($"RefreshUnlocks: {item.ItemName}");
        ProcessItemReceived(item);
      }
    }
  }

  public static void GetItems(ArchipelagoSession session) {
    session.Items.ItemReceived += (receivedItemsHelper) => {
      var itemReceivedName = receivedItemsHelper.PeekItem();

      // ... Handle item receipt here
      ProcessItemReceived(itemReceivedName);
      receivedItemsHelper.DequeueItem();
    };
  }

  public static void Connect(string server, string port, string user, string pass)
  {
    // Called whenever a new game starts
    session = ArchipelagoSessionFactory.CreateSession(server, Int32.Parse(port));

    // Must go BEFORE a successful connection attempt
    GetItems(session);

    attemptingConnection = true;
    session.MessageLog.OnMessageReceived += OnMessageReceived;
    LoginResult result;
    try
    {
      // handle TryConnectAndLogin attempt here and save the returned object to `result`
      result = session.TryConnectAndLogin("Exocolonist", user, ItemsHandlingFlags.AllItems);
      attemptingConnection = false;
      authenticated = true;
      serverData.uri = server;
      serverData.port = port;
      serverData.slotName = user;
      serverData.password = pass;
      serverData.StartNewSeed();
    }
    catch (Exception e)
    {
      result = new LoginFailure(e.GetBaseException().Message);
    }

    if (!result.Successful)
    {
      attemptingConnection = false;
      offline = true;
      LoginFailure failure = (LoginFailure)result;
      string errorMessage = $"Failed to Connect to {server} as {user}:";
      foreach (string error in failure.Errors)
      {
          errorMessage += $"\n    {error}";
      }
      foreach (ConnectionRefusedError error in failure.ErrorCodes)
      {
          errorMessage += $"\n    {error}";
      }
      Plugin.Logger.LogInfo("Connection error");
      Plugin.Logger.LogInfo(errorMessage);
      return;
    }
    
    var loginSuccess = (LoginSuccessful)result;
    Plugin.Logger.LogInfo("Successfully connected!");
  }

  public static void Disconnect()
  {
    authenticated = false;
    session.Socket.DisconnectAsync();
  }

  static void OnMessageReceived(LogMessage message)
  {
    Plugin.Logger.LogInfo(message.ToString());

    switch (message) {
      case HintItemSendLogMessage hintItemSendLogMessage:
        var hintReceiver = hintItemSendLogMessage.Receiver;
        var hintSender = hintItemSendLogMessage.Sender;
        var hintNetworkItem = hintItemSendLogMessage.Item;
        if (hintSender.Name == serverData.slotName || hintReceiver.Name == serverData.slotName) {
          Exopelago.Helpers.DisplayAPHint(hintSender.Name, hintReceiver.Name, hintNetworkItem.ItemName,hintNetworkItem.LocationDisplayName);
        }
        break;
      case ItemSendLogMessage itemSendLogMessage:
        var receiver = itemSendLogMessage.Receiver;
        var sender = itemSendLogMessage.Sender;
        var networkItem = itemSendLogMessage.Item;
        if (sender.Name == serverData.slotName) {
          Exopelago.Helpers.DisplayAPItem(sender.Name, receiver.Name, networkItem.ItemName);
        }
        break;
    }
  }

  static void ProcessItemReceived(ItemInfo item)
  {
    // This prints all properties of the item received, useful for debugging
    foreach(PropertyDescriptor descriptor in TypeDescriptor.GetProperties(item))
    {
      string name = descriptor.Name;
      object value = descriptor.GetValue(item);
      Plugin.Logger.LogInfo($"{name}={value}");
    }
    var itemName = item.ItemName;

    if (ItemsAndLocationsHandler.apToInternalJobs.ContainsKey(itemName)) {
      var internalName = ItemsAndLocationsHandler.apToInternalJobs[itemName];
      serverData.receivedJobs.Add(internalName);
      Plugin.Logger.LogInfo($"Attempting to unlock {itemName} - {internalName}");
      Exopelago.Helpers.UnlockJob(internalName);
    } 
    else if (ItemsAndLocationsHandler.apToInternalCollectibles.ContainsKey(itemName)){
      var internalName = ItemsAndLocationsHandler.apToInternalCollectibles[itemName];
      Plugin.Logger.LogInfo($"Attempting to unlock {itemName} - {internalName}");
      Exopelago.Helpers.GiveCard(internalName);
    } 
    else if (itemName == "Progressive Year") {
      Plugin.Logger.LogInfo($"Attempting to add a progressive year");
      serverData.RaiseAge();
    } 
    else if (ItemsAndLocationsHandler.apToInternalBuildings.ContainsKey(itemName)) {
      var internalName = ItemsAndLocationsHandler.apToInternalBuildings[itemName];
      Plugin.Logger.LogInfo($"Attempting to unlock {itemName} - {internalName}");
      serverData.receivedJobs.Add(internalName);
    } 
    else if (itemName.Contains("Perk")) {
      string skill = itemName.Replace("Progressive ", "").Replace(" Perk", "").ToLower();
      ArchipelagoClient.serverData.receivedPerk[skill]++;
      Exopelago.Helpers.UnlockPerk(skill);
    }
  }

  public static void ProcessLocation(string location) 
  {
    var locationId = session.Locations.GetLocationIdFromName("Exocolonist", location);
    session.Locations.CompleteLocationChecks(locationId);
  }

  public static void SendGoal()
  {
    // Move into wherever send goal is
    session.SetGoalAchieved();
  }

  public static string GetHog(string hog)
  {
    var output = session.DataStorage["hog"].To<Dictionary<string, string>>();
    string pretty = output.Aggregate(
      "{", 
      (str, kv) => str += $"\"{kv.Key}\": \"{kv.Value}\", ", 
      (str) => str += "}"
    );
    Plugin.Logger.LogInfo($"GetHog: {pretty}");
    if (output.ContainsKey(hog)) {
      Plugin.Logger.LogInfo($"Returning! {output[hog]}");
      return output[hog];
    } else {
      return null;
    }
  } 


  public static void SetHog(string hog, string value)
  {
    //Plugin.Logger.LogInfo(session.DataStorage["hog"]);
    var output = session.DataStorage["hog"].To<Dictionary<string, string>>();
    if (output == null) {
      session.DataStorage["hog"] = JObject.FromObject(new Dictionary<string, string>{});
      output = session.DataStorage["hog"].To<Dictionary<string, string>>();
    }
    session.DataStorage["hog"] += Operation.Update(new Dictionary<string, string>{{hog, value}});
    output = session.DataStorage["hog"].To<Dictionary<string, string>>();
    string pretty = output.Aggregate(
      "{", 
      (str, kv) => str += $"\"{kv.Key}\": \"{kv.Value}\", ", 
      (str) => str += "}"
    );
    Plugin.Logger.LogInfo($"SetHog: {pretty}");
  }


  public static Dictionary<string, string> GetAllHogs()
  {
    var output = session.DataStorage["hog"].To<StringDictionary>();
    string pretty = output.Aggregate(
      "{", 
      (str, kv) => str += $"\"{kv.Key}\": \"{kv.Value}\", ", 
      (str) => str += "}"
    );
    Plugin.Logger.LogInfo($"GetAllHogs: {pretty}");
    return output;
  }
}

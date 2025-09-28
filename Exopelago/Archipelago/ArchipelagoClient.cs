using System;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
//using Archipelago.MultiClient.Net.Exceptions;
//using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.MessageLog.Messages;
//using Archipelago.MultiClient.Net.Packets;
using Archipelago.MultiClient.Net.Models;

namespace Exopelago.Archipelago;
public static class ArchipelagoClient
{
  public const string GameName = "Exocolonist";
  public const string ModName = "Exopelago";
  public const string ModVersion = "0.2.0";

  public static bool authenticated;
  public static bool offline;
  public static ArchipelagoData serverData = new ();
  public static ArchipelagoSession session;

  public static bool readyForItems = false;


// ========== Connect ========== \\
  public static bool ConnectSavedInfo()
  {
    Connect(serverData.uri, serverData.port, serverData.slotName, serverData.password);
    if (authenticated) {
      return true;
    } else {
      return false;
    }
  }

  public static void Connect(string server, string port, string user, string pass)
  {
    session = ArchipelagoSessionFactory.CreateSession(server, int.Parse(port));

    session.MessageLog.OnMessageReceived += OnMessageReceived;
    LoginResult result;
    try
    {
      // handle TryConnectAndLogin attempt here and save the returned object to `result`
      result = session.TryConnectAndLogin("Exocolonist", user, ItemsHandlingFlags.AllItems);
      serverData.uri = server;
      serverData.port = port;
      serverData.slotName = user;
      serverData.password = pass;
      serverData.InitializeData();

      session.Items.ItemReceived += (receivedItemsHelper) => {
        var itemReceivedName = receivedItemsHelper.PeekItem();
        // ... Handle item receipt here
        ProcessItemReceived(itemReceivedName);
        receivedItemsHelper.DequeueItem();
      };

      while (session.Items.Any()) {
        session.Items.DequeueItem();
      }
    }
    catch (Exception e)
    {
      result = new LoginFailure(e.GetBaseException().Message);
    }
    if (!result.Successful)
    {
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
      Plugin.Logger.LogInfo($"Connection error: {errorMessage}");
      return;
    } else {
      authenticated = true;
      ArchipelagoData.GroundhogsFileNameBase = $"{serverData.slotName}-{serverData.seed}";
      Plugin.Logger.LogInfo("Successfully connected!");
    }

    var loginSuccess = (LoginSuccessful)result;
  }


// ========== Disconnect ========== \\
  public static void Disconnect()
  {
    authenticated = false;
    session.Socket.DisconnectAsync();
    ArchipelagoData.GroundhogsFileNameBase = "Groundhogs";
  }


// ========== Message handler ========== \\
  static void OnMessageReceived(LogMessage message)
  {
    Plugin.Logger.LogInfo($"AP says: {message.ToString()}");
    if (message is ItemSendLogMessage itemSendLogMessage && itemSendLogMessage.IsRelatedToActivePlayer) {
      Exopelago.Helpers.DisplayAPMessage(itemSendLogMessage);
    }
  }

// ========== Item Receiving ========== \\
  static void ProcessItemReceived(ItemInfo item)
  {
    // This prints all properties of the item received, useful for debugging
    //foreach(PropertyDescriptor descriptor in TypeDescriptor.GetProperties(item))
    //{
    //  string name = descriptor.Name;
    //  object value = descriptor.GetValue(item);
    //  Plugin.Logger.LogInfo($"{name}={value}");
    //}
    Plugin.Logger.LogInfo($"AP item received: {item.ItemDisplayName}");
    var itemName = item.ItemName;

    // Other cases to display the item received popup are handled in OnMessageReceived
    // But the server sending us an object never actually passes through that handler
    if (item.Player.Name == "Server") {
      Helpers.DisplayAPMessage($"Server sent you {itemName}");
    }

    // Is it a job?
    if (ItemsAndLocationsHandler.apToInternalJobs.ContainsKey(itemName)) {
      var internalName = ItemsAndLocationsHandler.apToInternalJobs[itemName];
      serverData.receivedJobs.Add(internalName);
      Plugin.Logger.LogInfo($"Attempting to unlock {itemName} - {internalName}");
      Exopelago.Helpers.UnlockJob(internalName);
    } 
    // Is it a collectible?
    else if (ItemsAndLocationsHandler.apToInternalCollectibles.ContainsKey(itemName)){
      var internalName = ItemsAndLocationsHandler.apToInternalCollectibles[itemName];
      Plugin.Logger.LogInfo($"Attempting to unlock {itemName} - {internalName}");
      Exopelago.Helpers.GiveCard(internalName);
    } 
    // Is it a year?
    else if (itemName == "Progressive Year") {
      Plugin.Logger.LogInfo($"Attempting to add a progressive year");
      serverData.RaiseAge();
    } 
    // Is it a building? (not implemented yet)
    else if (ItemsAndLocationsHandler.apToInternalBuildings.ContainsKey(itemName)) {
      var internalName = ItemsAndLocationsHandler.apToInternalBuildings[itemName];
      Plugin.Logger.LogInfo($"Attempting to unlock {itemName} - {internalName}");
      serverData.receivedJobs.Add(internalName);
    } 
    // Is it a perk?
    else if (itemName.Contains("Perk")) {
      string skill = itemName.Replace("Progressive ", "").Replace(" Perk", "").ToLower();
      ArchipelagoClient.serverData.receivedPerk[skill]++;
      Plugin.Logger.LogInfo($"Attempting to add a progressive {skill} perk");
      Exopelago.Helpers.UnlockPerk(skill);
    }
  }
  
  // TODO: Keep track of what index has already been processed in the save file
  public static void RefreshUnlocks(bool saveLoad = false) 
  {
    foreach (ItemInfo item in session.Items.AllItemsReceived) {
      if (saveLoad) {
        if (!ItemsAndLocationsHandler.apToInternalCollectibles.ContainsKey(item.ItemName) && !item.ItemName.Contains("Perk")) {
          Plugin.Logger.LogInfo($"RefreshUnlocks from save: {item.ItemName}");
          ProcessItemReceived(item);
        }
      } else {
        Plugin.Logger.LogInfo($"RefreshUnlocks from new game: {item.ItemName}");
        ProcessItemReceived(item);
      }
    }
  }


// ========== Location Sending ========== \\
  // For sending locations out
  public static void ProcessLocation(string location) 
  {
    var locationId = session.Locations.GetLocationIdFromName("Exocolonist", location);
    session.Locations.CompleteLocationChecks(locationId);
  }

  // Goal!
  public static void SendGoal()
  {
    Plugin.Logger.LogInfo($"Sending goal");
    session.SetGoalAchieved();
  }
}
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
  public static bool hasConnected;
  private static bool attemptingConnection;
  public static bool offline;
  public static ArchipelagoData serverData = new ();
  public static int offlineReceivedItems;
  public static ArchipelagoSession session;

  public static bool readyForItems = false;

  public static void GetItems(ArchipelagoSession session) {
    session.Items.ItemReceived += (receivedItemsHelper) => {
    var itemReceivedName = receivedItemsHelper.PeekItem();

    // ... Handle item receipt here
    ProcessItemReceived(itemReceivedName);
    receivedItemsHelper.DequeueItem();
    };
  }

  public static void Connect(string server, int port, string user, string pass)
  {
    // Called whenever a new game starts
    serverData.StartNewSeed();
    session = ArchipelagoSessionFactory.CreateSession(server, port);

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
      hasConnected = true;
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
      Plugin.Logger.LogInfo(errorMessage);
      return;
    }
    
    var loginSuccess = (LoginSuccessful)result;
    Plugin.Logger.LogInfo("Successfully connected!");
  }

  public static void Disconnect()
  {
    session.Socket.DisconnectAsync();
  }

  static void OnMessageReceived(LogMessage message)
  {
    Plugin.Logger.LogInfo(message.ToString());
  }

  static void ProcessItemReceived(ItemInfo item)
  {
    // This prints all properties of the item received, useful for debugging
    foreach(PropertyDescriptor descriptor in TypeDescriptor.GetProperties(item))
    {
      string name = descriptor.Name;
      object value = descriptor.GetValue(item);
      Console.WriteLine("{0}={1}", name, value);
    }
    // Not working yet, need to set up Exoloader deps for custom story
    //if (readyForItems){
    //  Princess.SetMemory("mem_lastPlayerRec", item.Player);
    //  Princess.SetMemory("mem_lastItemRec", item.ItemName);
    //  Story apStory = Story.FromID("apReceived");
    //  Result apResult = new Result();
    //  apStory.Execute(apResult);
    //}
    var itemName = item.ItemName;
    if (ItemsAndLocationsHandler.apToInternalJobs.ContainsKey(itemName)) {
      var internalName = ItemsAndLocationsHandler.apToInternalJobs[itemName];
      serverData.receivedJobs.Add(internalName);
      Plugin.Logger.LogInfo($"Attempting to unlock {itemName} - {internalName}");
      Exopelago.Princess_MemoryPatch.UnlockJob(internalName);
    } 
    else if (ItemsAndLocationsHandler.apToInternalCollectibles.ContainsKey(itemName)){
      var internalName = ItemsAndLocationsHandler.apToInternalCollectibles[itemName];
      Plugin.Logger.LogInfo($"Attempting to unlock {itemName} - {internalName}");
      Exopelago.Story_ExecutePatch.GiveCollectible(internalName);
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
  }

  public static void ProcessLocation(string location) 
  {
    var locationId = session.Locations.GetLocationIdFromName("Exocolonist", location);
    Plugin.Logger.LogInfo($"Trying to unlock location with name {location} id {locationId.ToString()}");
    session.Locations.CompleteLocationChecks(locationId);
  }

  public static void SendGoal()
  {
    session.SetClientState(ArchipelagoClientState.ClientGoal);
  }

  //Not properly used yet, this sets a variable in the server to fetch later
  public static bool SetHog(string hog, string value)
  {
    //Plugin.Logger.LogInfo(session.DataStorage["hog"]);
    session.DataStorage["hog"] = JObject.FromObject(new Dictionary<string, string>{{"one", "two"},{"three", "four"}});
    var output = session.DataStorage["hog"].To<Dictionary<string, string>>();
    var pretty = output.Aggregate(
      "{", 
      (str, kv) => str += $"\"{kv.Key}\": \"{kv.Value}\", ", 
      (str) => str += "}"
    );
    //Plugin.Logger.LogInfo(pretty);
    session.DataStorage["hog"] += Operation.Update(new Dictionary<string, string>{{"three", "five"}});
    output = session.DataStorage["hog"].To<Dictionary<string, string>>();
    pretty = output.Aggregate(
      "{", 
      (str, kv) => str += $"\"{kv.Key}\": \"{kv.Value}\", ", 
      (str) => str += "}"
    );
    //Plugin.Logger.LogInfo(pretty);
    return true;
  }
}

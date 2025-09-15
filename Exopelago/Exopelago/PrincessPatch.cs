using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Northway.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using Exopelago.Archipelago;

namespace Exopelago;

[HarmonyPatch(typeof(Princess))]
class Princess_MemoryPatch
{
  [HarmonyPatch("SetMemory")]
  [HarmonyPostfix]
  public static void Postfix(bool __runOriginal, string id, object value = null)
  {
    if (id.StartsWith("job_")){
      string strippedJob = id.RemoveStart("job_");
      Plugin.Logger.LogInfo($"Game tried to unlock {id}");
    } else {
    }
    Plugin.Logger.LogInfo($"Original memory ran: {__runOriginal}");
  }

  [HarmonyPatch("SetMemory")]
  [HarmonyPrefix]
  public static bool Prefix(string id, object value = null)
  {
    try {
      Plugin.Logger.LogInfo($"SetMemory prefix id {id}");
      return Helpers.ProcessMemory(id);
    } catch (Exception e) {
      // Magic try/catch block
      // The code works as intended with this here but never prints an error
      // Thanks Sae for the idea
      //string strippedJob = id.RemoveStart("job_");
      Plugin.Logger.LogInfo("ERROR");
      Plugin.Logger.LogInfo(id);
      //Plugin.Logger.LogInfo(strippedJob);
      Plugin.Logger.LogInfo(e);
      return true;
    } 
  }

}

[HarmonyPatch(typeof(PrincessMonth))]
class Princess_PrincessMonthPatch
{
  [HarmonyPatch("SetMonth")]
  [HarmonyPostfix]
  public static bool Prefix(int value)
  {
    int maxAge = ArchipelagoClient.serverData.maxAge;
    int maxMonth = (maxAge-10+1)*13+1;
    Plugin.Logger.LogInfo($"SetMonth prefix maxAge {maxAge} maxMonth {maxMonth} value {value}");
    if (value >= maxMonth) {
      Plugin.Logger.LogInfo($"Attempting to end the game");
      StoryCalls.endgame("archipelagoEnding");
    }
    return true;
  }
}

[HarmonyPatch(typeof(Princess))]
class Princess_PrincessInitPatch
{
  [HarmonyPatch("NewGame")]
  [HarmonyPostfix]
  public static void Postfix(bool __runOriginal)
  {
    // TODO: figure out how to set and get groundhogs here
  }
}

[HarmonyPatch(typeof(Princess))]
class Princess_LovePatch
{
  [HarmonyPatch("IncrementLove")]
  [HarmonyPostfix]
  public static void Postfix(string charaID, int diffAmount, Result result)
  {
    int loveInc = 20; // In case I make the increment user settable in the future
    int currentLove = Princess.GetLove(charaID);
    for (int i=1; i<=currentLove/loveInc; i++) {
      ArchipelagoClient.ProcessLocation($"{char.ToUpper(charaID[0]) + charaID.Substring(1)} {i*loveInc}");
    }
  }

}

[HarmonyPatch(typeof(PrincessCards))]
class Princess_PrincessCardPatch
{
  [HarmonyPatch("AddCard")]
  [HarmonyPostfix]
  public static bool Prefix(CardData cardData, Result result)
  {
    Plugin.Logger.LogInfo($"Adding {cardData.cardName} to the deck");
    switch (cardData.cardName) {
      case "Vriki":
        Plugin.Logger.LogInfo("Received Vriki card");
        ArchipelagoClient.ProcessLocation("Adopt Vriki");
        break;

      case "Vacubot":
        Plugin.Logger.LogInfo("Received Robot card");
        ArchipelagoClient.ProcessLocation("Adopt Robot");
        break;

      case "Hopeye":
        Plugin.Logger.LogInfo("Received Hopeye card");
        ArchipelagoClient.ProcessLocation("Adopt Hopeye");
        break;

      case "Unisaur":
        Plugin.Logger.LogInfo("Received Unisaur card");
        ArchipelagoClient.ProcessLocation("Adopt Unisaur");
        break;
      
      default:
        break;
    }
    return true;
  }

}
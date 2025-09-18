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
class Princess_SetMemoryPatch
{
  [HarmonyPatch("SetMemory")]
  [HarmonyPostfix]
  public static void Postfix(bool __runOriginal, string id, object value = null)
  {
    if (id.StartsWith("job_")){
      string strippedJob = id.RemoveStart("job_");
      //Plugin.Logger.LogInfo($"Game tried to unlock {id}");
    } else {
    }
    //Plugin.Logger.LogInfo($"{id} memory ran: {__runOriginal}");
  }

  [HarmonyPatch("SetMemory")]
  [HarmonyPrefix]
  public static bool Prefix(string id, object value = null)
  {
    try {
      //Plugin.Logger.LogInfo($"SetMemory prefix id {id}");
      return Helpers.ProcessMemory(id);
    } catch (Exception e) {
      // Magic try/catch block
      // The code works as intended with this here but never prints an error
      // Thanks Sae for the idea
      Plugin.Logger.LogInfo("ERROR");
      Plugin.Logger.LogInfo(id);
      Plugin.Logger.LogInfo(e);
      return true;
      // This then calls AddMemory
    } 
  }
}


[HarmonyPatch(typeof(Princess))]
class Princess_AddMemoryPatch
{
  [HarmonyPatch("AddMemory")]
  [HarmonyPrefix]
  public static bool Prefix(string id, object value = null)
  {
    try {
      if (ArchipelagoClient.serverData.perksanity){
        if (id.StartsWith("skillperk_")) {
          string perkID = id.Replace("skillperk_", "");
          string perk = perkID.Insert(perkID.Length - 1, " Perk ");
          string location = CultureInfo.CurrentCulture.TextInfo.ToTitleCase($"{perk}".ToLower());
          Plugin.Logger.LogInfo($"AddMemory location {id}");
          ArchipelagoClient.ProcessLocation(location);
          return false;
        } else if (id.StartsWith("unlockskillperk_")) {
          string perkID = id.Replace("unlock", "");
          Princess.memories.AddSafe(perkID, "true");
          Plugin.Logger.LogInfo($"AddMemory AddSafe {perkID}");
          Plugin.Logger.LogInfo($"Trying to unlock {perkID}");
          return false;
        } else {
          return true;
        }
      } else {
        return true;
      }
    } catch (Exception e) {
      // Magic try/catch block
      // The code works as intended with this here but never prints an error
      // Thanks Sae for the idea
      Plugin.Logger.LogInfo("ERROR in AddMemory");
      Plugin.Logger.LogInfo(id);
      Plugin.Logger.LogInfo(e);
      return true;
    } 
  }

}
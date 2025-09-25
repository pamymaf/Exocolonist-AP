using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Northway.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using Exopelago.Archipelago;

namespace Exopelago;


[HarmonyPatch]
class PrincessMonthPatch
{
  // Detect age up
  [HarmonyPatch(typeof(PrincessMonth), "SetMonth")]
  [HarmonyPrefix]
  public static bool Prefix(int value)
  {
    int maxAge = ArchipelagoClient.serverData.maxAge;
    int maxMonth = (maxAge-10+1)*13+1;
    Plugin.Logger.LogInfo($"SetMonth prefix: maxAge {maxAge} maxMonth {maxMonth} value {value}");
    if (value >= maxMonth) {
      Plugin.Logger.LogInfo($"Attempting to end the game");
      StoryCalls.endgame("archipelagoEnding");
    } else if (value == 66) {
      // These jobs unlock automatically when you turn 15
      ArchipelagoClient.ProcessLocation("Mourn");
      ArchipelagoClient.ProcessLocation("Rebuild");
    }
    return true;
  }
}

[HarmonyPatch]
class PrincessPatches
{
  // Intercept skill up and perk unlock attempts
  [HarmonyPatch(typeof(Princess), "SetSkill")]
  [HarmonyPrefix]
  public static bool Prefix(string skillID, int value, Result result)
  {
    Skill skill = Skill.FromID(skillID);
    int current = skill.value;
    try {
      if (ArchipelagoClient.serverData.perksanity){
        if (skillID != "stress" && skillID != "rebellion" && skillID != "kudos") {
          switch (value) {
            case >= 100:
              Princess.AddMemory($"skillperk_{skillID}3");
              break;
            case >= 67:
              Princess.AddMemory($"skillperk_{skillID}2");
              break;
            case >= 34:
              Princess.AddMemory($"skillperk_{skillID}1");
              break;
          }

          if (value >= 34 && !skill.HasSkillPerkLevel(1)) {
            Princess.SetSkill(skillID, 33, result);
            return false;
          } else if (value >= 67 && !skill.HasSkillPerkLevel(2)) {
            Princess.SetSkill(skillID, 66, result);
            return false;
          } else if (value >= 100 && !skill.HasSkillPerkLevel(3)) {
            Princess.SetSkill(skillID, 99, result);
            return false;
          } else {
            return true;
          }
        }
      } else {
        return true;
      }
    } catch (Exception e) {
      // Magic try/catch block
      // The code works as intended with this here but never prints an error
      // Thanks Sae for the idea
      Plugin.Logger.LogError($"SetSkill ID: {skillID} error: {e}");
      return true;
    } 
    return true;
  }

  // Detect love increment attempts
  [HarmonyPatch(typeof(Princess), "IncrementLove")]
  [HarmonyPostfix]
  public static void Postfix(string charaID, int diffAmount, Result result)
  {
    if (ArchipelagoClient.serverData.friendsanity){
      int loveInc = 20; // In case I make the increment user settable in the future
      int currentLove = Princess.GetLove(charaID);
      for (int i=1; i<=currentLove/loveInc; i++) {
        ArchipelagoClient.ProcessLocation($"{char.ToUpper(charaID[0]) + charaID.Substring(1)} {i*loveInc}");
      }
    }
  }
}

[HarmonyPatch(typeof(PrincessCards))]
class Princess_PrincessCardPatch
{
  // Detect when a card is added to the deck
  [HarmonyPatch("AddCard")]
  [HarmonyPrefix]
  public static bool Prefix(CardData cardData, Result result)
  {
    Plugin.Logger.LogInfo($"Adding {cardData.cardName} to the deck");
    switch (cardData.cardName) {
      case "Vriki":
        ArchipelagoClient.ProcessLocation("Adopt Vriki");
        break;

      case "Pet Bot":
        ArchipelagoClient.ProcessLocation("Adopt Robot");
        break;

      case "Hopeye":
        ArchipelagoClient.ProcessLocation("Adopt Hopeye");
        break;

      case "Unisaur":
        ArchipelagoClient.ProcessLocation("Adopt Unisaur");
        break;
      
      default:
        break;
    }
    return true;
  }
}

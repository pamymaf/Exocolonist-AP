using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Northway.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using Exopelago.Archipelago;

namespace Exopelago;

public static class Helpers
{
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
}
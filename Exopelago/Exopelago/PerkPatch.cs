using HarmonyLib;
using Exopelago.Archipelago;

namespace Exopelago;

[HarmonyPatch(typeof(PerkMenu))]
class Perk_MenuPatch
{
  [HarmonyPatch("ShowPerk")]
  [HarmonyPrefix]
  public static bool Prefix(Skill skill, int perkLevel)
  {
    if (ArchipelagoClient.serverData.perksanity) {
      return false; // TODO: Show perk popup when AP check comes in
    } else {
      return true;
    }
  }
}
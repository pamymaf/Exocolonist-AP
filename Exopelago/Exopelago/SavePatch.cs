using HarmonyLib;
using Newtonsoft.Json.Linq;
using Exopelago.Archipelago;

namespace Exopelago;

[HarmonyPatch]
class SavePatch
{
  [HarmonyPatch(typeof(Savegame), "Load2")]
  [HarmonyPostfix]
  public static void Load2Postfix(Savegame __instance)
  {
    string connectedSeed = ArchipelagoClient.serverData.seed;
    string connectedSlot = ArchipelagoClient.serverData.slotName;
    JObject saveJson = Helpers.GetConnectionInfoSaveGame();
    Helpers.firstMapLoad = true;
    Plugin.Logger.LogInfo($"Loaded save. Save info: {saveJson}");
    if (connectedSeed == (string)saveJson["apseed"] && connectedSlot == (string)saveJson["apslot"]){
      ArchipelagoClient.RefreshUnlocks(true);
    }
  }
}
using HarmonyLib;
using Newtonsoft.Json.Linq;
using Northway.Utils;
using Exopelago.Archipelago;

namespace Exopelago;

[HarmonyPatch(typeof(Player))]
class Player_ExecutePatch
{
  [HarmonyPatch("OnMapLoaded")]
  [HarmonyPostfix]
  public static void Postfix(bool startingOrLoadingGame, bool returningHome)
  {
    Helpers.readyForItems = true;
    Plugin.Logger.LogInfo("Map loaded");
    string connectedSeed = ArchipelagoClient.serverData.seed;
    string connectedSlot = ArchipelagoClient.serverData.slotName;
    JObject saveJson = Helpers.GetConnectionInfoSaveGame();
    if (Helpers.firstMapLoad){
      if ((string)saveJson["apseed"] == "") {
        Helpers.DisplayAPMessage("This is a vanilla save");
      } else if (connectedSeed != (string)saveJson["apseed"] || connectedSlot != (string)saveJson["apslot"]){
        Helpers.DisplayAPMessage("Invalid seed and slot name");
      }  else {
        Helpers.DisplayAPMessage();
        Singleton<SkillsMenu>.instance.ResetFillbarProgress();
      }
    }
    Helpers.firstMapLoad = false;
  }
}
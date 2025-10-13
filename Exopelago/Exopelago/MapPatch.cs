using HarmonyLib;
using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using Exopelago.Archipelago;

namespace Exopelago;

[HarmonyPatch(typeof(MapManager))]
class MapPatch
{
  // The locations of everyone in every season on every map
  public static Dictionary<string, Dictionary<string, Vector3>> stratoPos = new (){
    {"quiet", new ()
      {
        {"tammy", new Vector3(11.79f, -0.37f, 4.45f)},
        {"cal", new Vector3(35.20f, 2.92f, -22.34f)},
        {"anemone", new Vector3(-14.53f, -0.92f, -23.37f)},
        {"dad", new Vector3(28.07f, 2.94f, -17.52f)},
        {"mars", new Vector3(34.18f, 8.65f, 4.44f)},
        {"mom", new Vector3(25.56f, 2.89f, -18.62f)},
        {"tang", new Vector3(-10.81f, -0.65f, -7.07f)},
        {"dys", new Vector3(5.59f, 1.74f, -23.25f)}, 
      }
    },
    {"pollen", new ()
      {
        {"anemone", new Vector3(-5.86f, -1.09f, -26.33f)},
        {"cal", new Vector3(17.36f, -0.02f, -12.62f)},
        {"tang", new Vector3(-4.30f, -0.42f, -2.76f)},
        {"mars", new Vector3(25.78f, 4.75f, 3.77f)},
        {"tammy", new Vector3(15.54f, -0.11f, -12.77f)},
        {"dad", new Vector3(34.75f, 2.93f, -22.21f)},
        {"mom", new Vector3(37.12f, 2.90f, -22.98f)},
        {"dys", new Vector3(-1.62f, 0.81f, -20.08f)}, 
      }
    },
    {"dust", new ()
      {
        {"anemone", new Vector3(-10.79f, -0.92f, -20.47f)},
        {"cal", new Vector3(27.66f, 2.93f, -16.93f)},
        {"tang", new Vector3(-2.89f, -0.10f, -5.00f)},
        {"dys", new Vector3(27.84f, 2.85f, -33.95f)},
        {"mars", new Vector3(28.39f, 9.44f, 8.07f)},
        {"tammy", new Vector3(1.81f, 0.02f, 1.09f)},
        {"dad", new Vector3(28.08f, 2.90f, -25.95f)},
        {"mom", new Vector3(25.56f, 2.81f, -27.31f)}, 
      }
    },
    {"wet", new ()
      {
        {"anemone", new Vector3(-16.35f, -0.92f, -17.20f)},
        {"cal", new Vector3(10.33f, -0.85f, -15.21f)},
        {"tang", new Vector3(-11.93f, -0.43f, -7.68f)},
        {"mars", new Vector3(34.32f, 10.13f, 8.60f)},
        {"tammy", new Vector3(4.77f, -0.52f, -0.29f)},
        {"dad", new Vector3(29.05f, 2.85f, -33.27f)},
        {"mom", new Vector3(38.80f, 2.90f, -23.56f)},
        {"dys", new Vector3(13.20f, -0.02f, -11.67f)}, 
      }
    },
    {"glow", new ()
      {
        {"anemone", new Vector3(7.66f, -0.78f, -9.98f)},
        {"mom", new Vector3(9.44f, -0.86f, 1.73f)},
        {"dys", new Vector3(27.64f, 2.85f, -33.12f)}, 
      }
    },
  };

  public static Dictionary<string, Dictionary<string, Vector3>> ruinedPos = new(){
    {"quiet", new ()
      {
        {"cal", new Vector3(47.20f, 6.51f, -36.35f)},
        {"dys", new Vector3(41.05f, 6.30f, -48.83f)},
        {"mars", new Vector3(10.90f, 3.34f, -21.57f)},
        {"tang", new Vector3(9.30f, 2.90f, -19.37f)},
        {"mom", new Vector3(48.88f, 6.35f, -39.22f)},
        {"dad", new Vector3(40.00f, 6.35f, -30.47f)},
        {"anemone", new Vector3(-16.2f, -1.07f, -26)},
        {"tammy", new Vector3(5.43f, -0.82f, 3.47f)},
      }
    }
  };

  public static Dictionary<string, Dictionary<string, Vector3>> helioPos = new(){
    {"quiet", new ()
      {
        {"anemone", new Vector3(33.66f, 3.4f, -48.24f)},
        {"cal", new Vector3(-35.75f, 0.83f, 12.67f)},
        {"dys", new Vector3(-40.72f, 12.14f, -57.87f)},
        {"mars", new Vector3(-7.24f, 11.3f, -13.36f)},
        {"nomi", new Vector3(15.01f, 3.69f, -17.69f)},
        {"rex", new Vector3(16.8f, 3.8f, -17.35f)},
        {"tang", new Vector3(46.49f, 4.48f, -23.1f)},
        {"tammy", new Vector3(30.53f, 1.05f, 9.25f)},
        {"vace", new Vector3(8.23f, 3.4f, -43.79f)},
        {"mom", new Vector3(-54.07f, 3.23f, 5.35f)},
        {"dad", new Vector3(-51.91f, 2.65f, 5.5f)},
      }
    },
    {"pollen", new ()
      {
        {"anemone", new Vector3(5.31f, 3.4f, -43.82f)},
        {"cal", new Vector3(21.38f, 0.35f, 1.72f)},
        {"dys", new Vector3(-28.73f, 11.57f, -38.61f)},
        {"mars", new Vector3(-10.55f, 4.01f, -22.17f)},
        {"nomi", new Vector3(6.25f, 4.44f, -21.15f)},
        {"rex", new Vector3(-8.71f, 4.08f, -22.04f)},
        {"tang", new Vector3(40.83f, 4.03f, -23.77f)},
        {"tammy", new Vector3(20.38f, 0.4f, 1.39f)},
        {"vace", new Vector3(11.96f, 3.45f, -53.32f)},
        {"mom", new Vector3(-43.55f, 0.03f, 25.47f)},
        {"dad", new Vector3(-40.53f, 0.94f, 13.65f)},
      }
    },
    {"dust", new ()
      {
        {"anemone", new Vector3(38.99f, 3.4f, -48.73f)},
        {"cal", new Vector3(-43.04f, 3.4f, -11.75f)},
        {"dys", new Vector3(-40.06f, 12.06f, -65.76f)},
        {"mars", new Vector3(-4.54f, 11.78f, -10.49f)},
        {"nomi", new Vector3(31.35f, 1.0f, -6.66f)},
        {"rex", new Vector3(-4.82f, 3.93f, -17.95f)},
        {"tang", new Vector3(54.52f, 6.19f, -27.67f)},
        {"tammy", new Vector3(34.22f, 0.62f, 6.42f)},
        {"vace", new Vector3(16.24f, 3.45f, -54.29f)},
        {"mom", new Vector3(-38.67f, 0.4f, 17.86f)},
        {"dad", new Vector3(-55.06f, 3.51f, -9.0f)},
      }
    },
    {"wet", new ()
      {
        {"anemone", new Vector3(6.38f, 3.4f, -51.17f)},
        {"cal", new Vector3(-43.98f, 0.4f, 18.5f)},
        {"dys", new Vector3(-46.37f, 11.76f, -66.26f)},
        {"mars", new Vector3(-7.47f, 3.7f, -16.84f)},
        {"nomi", new Vector3(-43.82f, 11.8f, -64.16f)},
        {"rex", new Vector3(-5.75f, 3.72f, -15.47f)},
        {"tang", new Vector3(47.86f, 4.31f, -22.71f)},
        {"tammy", new Vector3(24.31f, 0.7f, 3.18f)},
        {"vace", new Vector3(7.47f, 3.44f, -52.17f)},
        {"mom", new Vector3(-51.31f, 3.41f, -6.66f)},
        {"dad", new Vector3(-48.93f, 3.16f, 11.77f)},
      }
    },
    {"glow", new()
      {
        {"anemone", new Vector3(13.74f, 3.45f, -51.89f)},
        {"cal", new Vector3(27.83f, 0.35f, 8.13f)},
        {"dys", new Vector3(9.35f, 5.87f, -23.15f)},
        {"tang", new Vector3(48.47f, 4.29f, -22.16f)},
        {"vace", new Vector3(14.79f, 3.45f, -52.7f)},
        {"mom", new Vector3(35.16f, 0.85f, 9.37f)},
        {"dad", new Vector3(36.66f, 0.69f, 8.97f)},
      }
    }
  };

  public static Dictionary<string, Vector3> stratoBuldingEntrancePos = new() {
    {"quarters", new Vector3(5f, -1f, 8f)},
    {"engineering", new Vector3(-9f, -1f, -2f)},
    {"garrison", new Vector3(-14f, -1f, -17f)},
    {"expeditions", new Vector3(11f, -1f, -24f)},
    {"geoponics", new Vector3(32f, 3.33f, -17f)},
    {"command", new Vector3(31f, 11f, 12f)},
  };

  public static Dictionary<string, Vector3> helioBuildingEntrancePos = new () {
    {"quarters", new Vector3(10f, -3f, 25f)},
    {"engineering", new Vector3(39f, 1f, -5f)},
    {"garrison", new Vector3(-6f, 0f, -38f)},
    {"expeditions", new Vector3(-58f, 8.5f, -48f)},
    {"geoponics", new Vector3(-65f, -1f, 34f)},
    {"command", new Vector3(-20f, 9f, 5f)},
  };
  
  [HarmonyPatch("OnMonthChanged")]
  [HarmonyPrefix]
  public static void MonthChangePrefix(MapManager __instance)
  {
    Plugin.Logger.LogInfo($"In MonthChange Prefix");

    // Flag rando
    if (ArchipelagoClient.serverData.building_rando) {
      string scene = MapManager.currentScene;
      Vector3 warpDest;
      if (scene.ToLower() != "colonystratodestroyed") {
        foreach (string building in ArchipelagoClient.serverData.buildings.Keys) {
          if (ExopelagoSettings.settingBools["matchFlags"]) {
            string targetBuilding = ArchipelagoClient.serverData.buildings[building];
            targetBuilding = char.ToUpper(targetBuilding[0]) + targetBuilding.Substring(1);
            string origBuilding = char.ToUpper(building[0]) + building.Substring(1);
            string targetFlag = $"/Interactives/FlagMarkers/FlagMarker{origBuilding}/{targetBuilding}FlagSprite";
            string origFlag = $"/Interactives/FlagMarkers/FlagMarker{origBuilding}/{origBuilding}FlagSprite";
            if (building == "expeditions" && scene.ToLower() == "colonystrato") {
              targetFlag = $"/Interactives/Expeditions/month3plusGazebo/FlagMarkerExpeditions/{targetBuilding}FlagSprite";
              origFlag = $"/Interactives/Expeditions/month3plusGazebo/FlagMarkerExpeditions/{origBuilding}FlagSprite";
            }
            GameObject targetFlagSprite = GameObject.Find(targetFlag);
            GameObject origFlagSprite = GameObject.Find(origFlag);
            origFlagSprite.SetActive(false);
            targetFlagSprite.SetActive(true);
          }

          // Warp spot fix
          if (ArchipelagoClient.serverData.buildings[building] == "expeditions") {
            if (scene.ToLower() == "colonystrato") {
              warpDest = stratoBuldingEntrancePos[building];
              GameObject gameObject = GameObject.Find("/Interactives/entranceReturnFromExploreGlow");
              gameObject.transform.position = warpDest;
              gameObject = GameObject.Find("/Interactives/entranceReturnFromExpedition");
              gameObject.transform.position = warpDest;
              gameObject = GameObject.Find("/Interactives/entranceReturnFromSneak");
              gameObject.transform.position = warpDest;
            } else if (scene.ToLower() == "colonyhelio") {
              warpDest = helioBuildingEntrancePos[building];
              GameObject gameObject = GameObject.Find("/Interactives/entranceReturnFromExploreGlow");
              gameObject.transform.position = warpDest;
              gameObject = GameObject.Find("/Interactives/entranceReturnFromExpedition");
              gameObject.transform.position = warpDest;
            }
          }
        }
      }
    }

    if (ArchipelagoClient.serverData.character_rando) {
      //Chara rando logic here
      string season = Princess.season.seasonID;
      string scene = MapManager.currentScene;
      int seed = Tuple.Create(ArchipelagoClient.serverData.seed, ArchipelagoClient.serverData.slotName).GetHashCode();
      System.Random rng = new System.Random(seed);
      string path;
      GameObject gameObject;

      // Normal colony
      if (scene.ToLower() == "colonystrato") {
        Dictionary<string, string> refDict = new ();
        foreach (var chara in stratoPos[season]) {
          refDict[chara.Key] = chara.Key;
        }
        refDict = Helpers.RandomizeDict(refDict, seed);
        foreach (var chara in stratoPos[season]) {
          string charaID = chara.Key;
          string newCharaID = refDict[charaID];
          Vector3 newLoc = stratoPos[season][newCharaID];
          if (season == "quiet") {
            path = $"/Seasonal/quiet/inner/week3plus/chara_{charaID}/";
            gameObject = GameObject.Find(path);
            gameObject.transform.position = newLoc;
          } else if (season == "glow") {
            path = $"/Seasonal/glow/inner/charas/chara_{charaID}";
            gameObject = GameObject.Find(path);
            gameObject.transform.position = newLoc;
          } else {
            // It's possible for Dys to be outside the walls in Pollen and Wet
            // We need to detect if he's outside or inside, even if we always place him inside
            if (charaID == "dys") {
              if (season == "pollen" || season == "wet") {
                gameObject = GameObject.Find($"/Seasonal/{season}/toggleDysSurvey/");
                if (gameObject.activeSelf) {
                  path = $"/Seasonal/{season}/toggleDysSurvey/chara_dys";
                  gameObject = GameObject.Find(path);
                  gameObject.transform.position = newLoc;
                } else {
                  path = $"/Seasonal/{season}/toggleDysNoSurvey/chara_dys";
                  gameObject = GameObject.Find(path);
                  gameObject.transform.position = newLoc;
                }
              } else {
                path = $"/Seasonal/{season}/chara_{charaID}/";
                gameObject = GameObject.Find(path);
                gameObject.transform.position = newLoc;
              }
            } else {
              path = $"/Seasonal/{season}/chara_{charaID}/";
              gameObject = GameObject.Find(path);
              gameObject.transform.position = newLoc;
            }
          }
        }

        
      }
      // Ruined colony
      else if (scene.ToLower() == "colonystratodestroyed") {
        Dictionary<string, string> refDict = new ();
        foreach (var chara in ruinedPos[season]) {
          refDict[chara.Key] = chara.Key;
        }
        refDict = Helpers.RandomizeDict(refDict, seed);
        foreach (var chara in ruinedPos[season]) {
          string charaID = chara.Key;
          string newCharaID = refDict[charaID];
          Vector3 newLoc = ruinedPos[season][newCharaID];
          if (charaID == "anemone" || charaID == "tammy") {
            path = $"/Seasonal/SeasonalEffects/month67plus/chara_{charaID}/";
            gameObject = GameObject.Find(path);
            gameObject.transform.localPosition = newLoc;
          } else {
            if (charaID == "mars") charaID = "marz";
            path = $"/Seasonal/SeasonalEffects/chara_{charaID}";
            gameObject = GameObject.Find(path);
            gameObject.transform.localPosition = newLoc;
          }
        }
      }
      // Helio
      else if (scene.ToLower() == "colonyhelio") {
        Dictionary<string, string> refDict = new ();
        foreach (var chara in helioPos[season]) {
          refDict[chara.Key] = chara.Key;
        }
        refDict = Helpers.RandomizeDict(refDict, seed);
        foreach (var chara in helioPos[season]) {
          string charaID = chara.Key;
          string newCharaID = refDict[charaID];
          Vector3 newLoc = helioPos[season][newCharaID];
          
          if (season == "glow") {
            path = $"/Seasonal/glow/inner/chara_{charaID}";
            gameObject = GameObject.Find(path);
            gameObject.transform.localPosition = newLoc;
          } else {
            path = $"/Seasonal/{season}/chara_{charaID}/";
            gameObject = GameObject.Find(path);
            gameObject.transform.localPosition = newLoc;
          }
        }
      }
    }
  }
}

[HarmonyPatch(typeof(RequirementToggler))]
class ReqTogglePatch
{
  [HarmonyPatch("CheckReqs")]
  [HarmonyPostfix]
  public static void Postfix(ref bool __result, RequirementToggler __instance)
  {
    if (__instance.GetPath().Contains("/Interactives/Expeditions/glowGate/") && Princess.monthOfGame < 3) {
      __result = true;
    } else if (__instance.GetPath().Contains("/3DModels/GatesGlow/") || (__instance.GetPath().Contains("/Interactives/Expeditions/glowGate/") && Princess.monthOfGame >= 3)) {
      __result = false;
    } else if (__instance.GetPath() == "/Seasonal/quiet/inner/week3plus/RequirementToggler") {
      __result = true;
    } else if (__instance.GetPath().Contains("/Seasonal/quiet/inner/week1/")) {
      __result = false;
    } else if (__instance.GetPath().Contains("/Seasonal/quiet/inner/week2/")) {
      __result = false;
    }
  }
}
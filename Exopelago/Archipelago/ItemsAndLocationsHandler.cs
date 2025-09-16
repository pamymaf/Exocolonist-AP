using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Exceptions;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.MessageLog.Messages;
using Archipelago.MultiClient.Net.Packets;
using Exopelago;

namespace Exopelago.Archipelago;

public static class ItemsAndLocationsHandler
{
  public static ReadOnlyDictionary<string, string> apToInternalJobs = new (new Dictionary<string, string>{
      {"Shovelling Dirt", "shovel"},
      {"Farming", "farm"},
      {"Xenobotany", "analyzePlants"},
      {"Tending Animals", "tendAnimals"},
      {"Relax in the Park", "relaxPark"},
      {"Babysitting", "babysitting"},
      {"Cooking", "cooking"},
      {"Barista", "barista"},
      {"Play the Photophonor", "photophonor"},
      {"Relax in the Lounge", "relaxLounge"},
      {"Study Life Sciences", "studyBiology"},
      {"Study Engineering", "studyPhysics"},
      {"Study Humanities", "studyWriting"},
      {"Tutoring", "tutoring"},
      {"Robot Repair", "robotRepair"},
      {"Nursing Assistant", "nurse"},
      {"Sportsball", "sportsball"},
      {"Defense Training", "defenseTraining"},
      {"Lookout Duty", "lookoutDuty"},
      {"Guard Duty", "guardDuty"},
      {"Relax on the Walls", "relaxWalls"},
      {"Deliver Supplies", "delivery"},
      {"Depot Clerk", "depot"},
      {"Construction", "construction"},
      {"Administration", "assistant"},
      {"Leader", "leader"},
      {"Sneak Out", "sneak"},
      {"Explore Nearby", "exploreNearby"},
      {"Survey the Plains", "survey"},
      {"Forage in the Valley", "forage"},
      {"Survey the Ridge", "artifacts"},
      {"Hunt in the Swamps", "hunt"},
      {"Explore Glow", "exploreGlow"},
      {"Rebuild", "rebuild"},
      {"Mourn", "mourn"},
    });

  public static ReadOnlyDictionary<string, string> internalToAPJobs = new (new Dictionary<string, string>{
    {"shovel", "Shovelling Dirt"},
    {"farm", "Farming"},
    {"analyzeplants", "Xenobotany"},
    {"tendanimals", "Tending Animals"},
    {"relaxpark", "Relax in the Park"},
    {"babysitting", "Babysitting"},
    {"cooking", "Cooking"},
    {"barista", "Barista"},
    {"photophonor", "Play the Photophonor"},
    {"relaxlounge", "Relax in the Lounge"},
    {"studybiology", "Study Life Sciences"},
    {"studyphysics", "Study Engineering"},
    {"studywriting", "Study Humanities"},
    {"tutoring", "Tutoring"},
    {"robotrepair", "Robot Repair"},
    {"nurse", "Nursing Assistant"},
    {"sportsball", "Sportsball"},
    {"defensetraining", "Defense Training"},
    {"lookoutduty", "Lookout Duty"},
    {"guardduty", "Guard Duty"},
    {"relaxwalls", "Relax on the Walls"},
    {"delivery", "Deliver Supplies"},
    {"depot", "Depot Clerk"},
    {"construction", "Construction"},
    {"assistant", "Administration"},
    {"leader", "Leader"},
    {"sneak", "Sneak Out"},
    {"explorenearby", "Explore Nearby"},
    {"survey", "Survey the Plains"},
    {"forage", "Forage in the Valley"},
    {"artifacts", "Survey the Ridge"},
    {"hunt", "Hunt in the Swamps"},
    {"exploreglow", "Explore Glow"},
    {"rebuild", "Rebuild"},
    {"mourn", "Mourn"},
  });

  public static ReadOnlyDictionary<string, string> apToInternalCollectibles = new (new Dictionary<string, string>{
    {"Mushwood Log", "wood"},
    {"Xeno Egg", "egg"},
    {"Bobberfruit", "fruit"},
    {"Crystal Cluster", "crystal"},
    {"Medicinal Roots", "roots"},
    {"Yellow Flower", "yellowFlower"},
    {"Strange Device", "device"},
    {"Cake", "cake"},
  });

  public static ReadOnlyDictionary<string, string> internalToAPcollectibles = new (new Dictionary<string, string>{
    {"wood", "Mushwood Log"},
    {"egg", "Xeno Egg"},
    {"fruit", "Bobberfruit"},
    {"crystal", "Crystal Cluster"},
    {"roots", "Medicinal Roots"},
    {"flower", "Yellow Flower"},
    {"device", "Strange Device"},
    {"cake", "Cake"},
  });

  public static ReadOnlyDictionary<string, string> internalToAPBuildings = new (new Dictionary<string, string>{
    {"garrison", "Garrison"},
    {"engineering", "Engineering"},
    {"quarters", "Living Quarters"},
    {"command", "Command"},
    {"geoponics", "Geoponics"},
    {"expeditions", "Expeditions"},
  });

  public static ReadOnlyDictionary<string, string> apToInternalBuildings = new (new Dictionary<string, string>{
    {"Garrison", "garrison"},
    {"Engineering", "engineering"},
    {"Living Quarters", "quarters"},
    {"Command", "command"},
    {"Geoponics", "geoponics"},
    {"Expeditions", "expeditions"},
  });

  public static Dictionary<string, string> nonDateEvents = new () {
    {"saved_tammy", "Save Tammy"},
    {"savedtonin", "Save Tonin"},
    {"savedeudicot", "Save Eudicot"},
    {"foodsaved", "Save Mom"},
    {"shimmercure", "Save Dad"},
    {"halsaved", "Save Hal"},
    {"leader_marz", "Marz Governor"},
    {"leader_player", "Become Governor"},
  };

  public static Dictionary<string, string> dateEvents = new () {
    {"date_anemone", "Date Anemone"},
    {"date_cal", "Date Cal"},
    {"date_dys", "Date Dys"},
    {"date_marz", "Date Marz"},
    {"date_nomi", "Date Nomi"},
    {"date_tammy", "Date Tammy"},
    {"date_tangent", "Date Tangent"},
    {"date_rex", "Date Rex"},
    {"date_vace", "Date Vace"},
  };

  public static ReadOnlyDictionary<string, string> storyEvents = new (new Dictionary<string, string>());
}
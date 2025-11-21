using System.Collections.ObjectModel;
using System.Collections.Generic;

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
    {"shovel", "Unlock Shovelling Dirt"},
    {"farm", "Unlock Farming"},
    {"analyzeplants", "Unlock Xenobotany"},
    {"tendanimals", "Unlock Tending Animals"},
    {"relaxpark", "Unlock Relax in the Park"},
    {"babysitting", "Unlock Babysitting"},
    {"cooking", "Unlock Cooking"},
    {"barista", "Unlock Barista"},
    {"photophonor", "Unlock Play the Photophonor"},
    {"relaxlounge", "Unlock Relax in the Lounge"},
    {"studybiology", "Unlock Study Life Sciences"},
    {"studyphysics", "Unlock Study Engineering"},
    {"studywriting", "Unlock Study Humanities"},
    {"tutoring", "Unlock Tutoring"},
    {"robotrepair", "Unlock Robot Repair"},
    {"nurse", "Unlock Nursing Assistant"},
    {"sportsball", "Unlock Sportsball"},
    {"defensetraining", "Unlock Defense Training"},
    {"lookoutduty", "Unlock Lookout Duty"},
    {"guardduty", "Unlock Guard Duty"},
    {"relaxwalls", "Unlock Relax on the Walls"},
    {"delivery", "Unlock Deliver Supplies"},
    {"depot", "Unlock Depot Clerk"},
    {"construction", "Unlock Construction"},
    {"assistant", "Unlock Administration"},
    {"leader", "Unlock Leader"},
    {"sneak", "Unlock Sneak Out"},
    {"explorenearby", "Unlock Explore Nearby"},
    {"survey", "Unlock Survey the Plains"},
    {"forage", "Unlock Forage in the Valley"},
    {"artifacts", "Unlock Survey the Ridge"},
    {"hunt", "Unlock Hunt in the Swamps"},
    {"exploreglow", "Unlock Explore Glow"},
    {"rebuild", "Unlock Rebuild"},
    {"mourn", "Unlock Mourn"},
  });

  public static ReadOnlyDictionary<string, string> apToInternalCollectibles = new (new Dictionary<string, string> {
    {"Mushwood Log", "wood"},
    {"Xeno Egg", "egg"},
    {"Bobberfruit", "fruit"},
    {"Crystal Cluster", "crystal"},
    {"Medicinal Roots", "roots"},
    {"Yellow Flower", "yellowFlower"},
    {"Strange Device", "device"},
    {"Cake", "cake"},
  });

  public static ReadOnlyDictionary<string, string> internalToAPcollectibles = new (new Dictionary<string, string> {
    {"wood", "Pick up Mushwood Log"},
    {"egg", "Pick up Xeno Egg"},
    {"fruit", "Pick up Bobberfruit"},
    {"crystal", "Pick up Crystal Cluster"},
    {"roots", "Pick up Medicinal Roots"},
    {"flower", "Pick up Yellow Flower"},
    {"device", "Pick up Strange Device"},
    {"cake", "Buy Cake"},
  });

  public static ReadOnlyDictionary<string, string> internalToAPBuildings = new (new Dictionary<string, string> {
    {"garrison", "Garrison"},
    {"engineering", "Engineering"},
    {"quarters", "Living Quarters"},
    {"command", "Command"},
    {"geoponics", "Geoponics"},
    {"expeditions", "Expeditions"},
  });

  public static ReadOnlyDictionary<string, string> apToInternalBuildings = new (new Dictionary<string, string> {
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
    {"date_tang", "Date Tang"},
    {"date_rex", "Date Rex"},
    {"date_vace", "Date Vace"},
    {"date_sym", "Date Sym"},
  };

  public static ReadOnlyDictionary<string, string> storyEvents = new (new Dictionary<string, string>());
}
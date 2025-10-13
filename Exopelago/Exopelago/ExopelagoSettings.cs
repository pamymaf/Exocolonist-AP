using System.Collections.Generic;

namespace Exopelago;

public class ExopelagoSettings
{
  public static Dictionary<string, string> settingNames = new () {
    {"matchFlags", "Match flags to entrances"},
  };

  public static Dictionary<string, string> settingDescriptions = new () {
    {"matchFlags", "Match building flags to the randomized entrances. Reload required if toggled during gameplay."},
  };

  public static Dictionary<string, bool> settingBools = new () {
    {"matchFlags", false},
  };
}
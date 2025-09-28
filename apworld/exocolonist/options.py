from dataclasses import dataclass

from Options import Choice, OptionGroup, PerGameCommonOptions, Range, Toggle

class Friendsanity(Toggle):
  """
  Add checks for befriending each character.
  Each multiple of 20 friendship points is a check.
  """
  display_name = "Friendsanity"

class Datesanity(Toggle):
  """
  Add checks for dating each character.
  """
  display_name = "Datesanity"

class Perksanity(Toggle):
  """
  Add checks for getting each skill perk.
  Will disable your ability to level past each perk and shuffle perks into the pool
  """
  display_name = "Perksanity"

class Ending(Choice):
  """
  Which endings should be considered a goal?
  """
  display_name = "Ending"

  option_any = 0
  option_no_slacker = 1
  option_special = 2

  default = option_any

class BuildingRando(Toggle):
  """
  Should buildings be randomized with each other?
  """
  display_name = "Building Randomization"


@dataclass
class ExocolonistOptions(PerGameCommonOptions):
  friendsanity: Friendsanity
  datesanity: Datesanity
  ending: Ending
  perksanity: Perksanity
  building_rando: BuildingRando


option_groups = [
  OptionGroup(
    "Friendship Options",
    [Friendsanity, Datesanity],
  ),
]


option_presets = {
  "default": {
    "friendsanity": True,
    "datesanity": True,
    "ending": Ending.option_no_slacker,
    "perksanity": True,
    "building_rando": False
  },
  "friendless": {
    "friendsanity": False,
    "datesanity": False,
    "ending": Ending.option_no_slacker,
    "perksanity": True,
    "building_rando": False
  },
}

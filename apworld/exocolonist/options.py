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

class Ending(Choice):
    """
    Which endings should be considered a goal?
    """
    display_name = "Ending"

    option_any = 0
    option_no_slacker = 1
    option_special = 2

    default = option_any

@dataclass
class ExocolonistOptions(PerGameCommonOptions):
    friendsanity: Friendsanity
    datesanity: Datesanity
    ending: Ending


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
        "ending": Ending.option_no_slacker
    },
    "friendless": {
        "friendsanity": False,
        "datesanity": False,
        "ending": Ending.option_no_slacker
    },
}

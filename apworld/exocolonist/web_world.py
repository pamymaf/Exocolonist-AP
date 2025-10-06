from BaseClasses import Tutorial
from worlds.AutoWorld import WebWorld
#from .options import option_groups, option_presets

class ExocolonistWebWorld(WebWorld):
  game = "I Was a Teenage Exocolonist"

  theme = "grassFlowers"

  setup_en = Tutorial(
    "Multiworld Setup Guide",
    "A guide to setting up I Was a Teenage Exocolonist for MultiWorld.",
    "English",
    "setup_en.md",
    "setup/en",
    ["pamymaf"],
  )

  tutorials = [setup_en]  


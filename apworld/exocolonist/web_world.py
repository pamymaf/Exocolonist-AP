from BaseClasses import Tutorial

from worlds.AutoWorld import WebWorld

#from .options import option_groups, option_presets


# For our game to display correctly on the website, we need to define a WebWorld subclass.
class ExocolonistWebWorld(WebWorld):
    game = "Exocolonist"

    theme = "grassFlowers"

    setup_en = Tutorial(
        "Multiworld Setup Guide",
        "A guide to setting up Exocolonist for MultiWorld.",
        "English",
        "setup_en.md",
        "setup/en",
        ["NewSoupVi"],
    )

    tutorials = [setup_en]  


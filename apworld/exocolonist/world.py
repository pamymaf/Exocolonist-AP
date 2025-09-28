from collections.abc import Mapping
from typing import Any

# Imports of base Archipelago modules must be absolute.
from worlds.AutoWorld import World

# Imports of your world's files must be relative.
from . import items, locations, regions, rules
from .options import ExocolonistOptions
from .web_world import ExocolonistWebWorld


# The world class is the heart and soul of an apworld implementation.
# It holds all the data and functions required to build the world and submit it to the multiworld generator.
# You could have all your world code in just this one class, but for readability and better structure,
# it is common to split up world functionality into multiple files.
# This implementation in particular has the following additional files, each covering one topic:
# regions.py, locations.py, rules.py, items.py, options.py and web_world.py.
# It is recommended that you read these in that specific order, then come back to the world class.
class ExocolonistWorld(World):
  """
  Spend your teenage years on an alien planet in this narrative RPG with card-based battles. 
  Explore, grow up, and fall in love. The choices you make and skills you master over ten years will determine the course of your life and the survival of your colony.
  """
  # TODO Write my own desciption

  game = "Exocolonist"
  web = ExocolonistWebWorld()
  options: ExocolonistOptions
  options_dataclass = ExocolonistOptions
  ut_can_gen_without_yaml = True

  location_name_to_id = locations.LOCATION_NAME_TO_ID
  item_name_to_id = items.ITEM_NAME_TO_ID

  origin_region_name = "Start"

  skills_to_job: dict[str, tuple[str, ...]] = {
    "empathy": ("Babysitting", "Barista", "Tending Animals", "Nursing Assistant", "Cooking"),
    "persuasion": ("Administration", "Leader", "Study Humanities", "Depot Clerk"),
    "creativity": ("Play the Photophonor", "Cooking", "Study Humanities", "Barista", "Robot Repair"),
    "bravery": ("Sportsball", "Guard Duty"), # Not adding Explore Glow as that can only happen once a year
    # These jobs give bravery, but also require bravery, so excluding them:  "Sneak Out", "Explore Nearby",
    
    "reasoning": ("Tutoring", "Study Engineering"),
    "organizing": ("Deliver Supplies", "Depot Clerk", "Xenobotany", "Administration"), # Not adding Rebuild as that is hard coded to year 15 only
    # Some places in game code use "organizing", others use "organization"
    "organization": ("Deliver Supplies", "Depot Clerk", "Xenobotany", "Administration"), # Not adding Rebuild as that is hard coded to year 15 only
    "engineering": ("Robot Repair", "Study Engineering", "Construction", "Survey the Ridge"),
    "biology": ("Nursing Assistant", "Xenobotany", "Study Life Sciences", "Farming", "Forage in the Valley"),

    "toughness": ("Shovelling Dirt", "Farming"), # Not adding Rebuild as that is hard coded to year 15 only
    "perception": ("Lookout Duty", "Survey the Plains", "Forage in the Valley", "Survey the Ridge"),
    "combat": ("Defense Training", "Guard Duty", "Hunt in the Swamps"),
    "animals": ("Hunt in the Swamps", "Tending Animals"),
  }

  safe_skills_to_job: dict[str, tuple[str, ...]] = {
    # This removes exploration jobs to make some logic easier
    "empathy": ("Babysitting", "Barista", "Tending Animals", "Nursing Assistant", "Cooking"),
    "persuasion": ("Administration", "Leader", "Study Humanities", "Depot Clerk"),
    "creativity": ("Play the Photophonor", "Cooking", "Study Humanities", "Barista", "Robot Repair"),
    "bravery": ("Sportsball", "Guard Duty"), # Not adding Explore Glow as that can only happen once a year
    # These jobs give bravery, but also require bravery, so excluding them:  "Sneak Out", "Explore Nearby",
    
    "reasoning": ("Tutoring", "Study Engineering"),
    "organizing": ("Deliver Supplies", "Depot Clerk", "Xenobotany", "Administration", "Rebuild"),
    # Some places in game code use "organizing", others use "organization"
    "organization": ("Deliver Supplies", "Depot Clerk", "Xenobotany", "Administration", "Rebuild"), 
    "engineering": ("Robot Repair", "Study Engineering", "Construction"),
    "biology": ("Nursing Assistant", "Xenobotany", "Study Life Sciences", "Farming"),

    "toughness": ("Shovelling Dirt", "Farming", "Rebuild"),
    "perception": ("Lookout Duty"),
    "combat": ("Defense Training", "Guard Duty"),
    "animals": ("Tending Animals"),
  }

  building_jobs: dict[str, tuple[str, ...]] = {
    "garrison": ("Sportsball", "Defense Training", "Guard Duty", "Lookout Duty", "Relax on the Walls"),
    "engineering": ("Nursing Assistant", "Study Life Sciences", "Study Humanitites", "Robot Repair", "Study Engineering", "Tutoring"),
    "quarters": ("Play the Photophonor", "Cooking", "Babysitting", "Barista", "Relax in the Lounge"),
    "command": ("Leader", "Construction", "Deliver Supplies", "Depot Clerk", "Administration"),
    "geoponics": ("Tending Animals", "Xenobotany", "Farming", "Shovelling Dirt", "Relax in the Park"),
    "expeditions": ("Hunt in the Swamps", "Sneak Out", "Explore Nearby", "Explore Glow", "Survey the Plains", "Forage in the Valley", "Survey the Ridge"),
  }

  chara_jobs: dict[str, tuple[str, ...]] = {
    "Anemone": ("Sportsball", "Lookout Duty", "Defense Training"),
    "Cal": ("Shovelling Dirt", "Farming", "Xenobotany", "Tending Animals"),
    "Dys": ("Sneak Out", "Explore Nearby", "Survey the Plains", "Survey the Ridge", "Relax on the Walls"),
    "Marz": ("Depot Clerk", "Administration", "Leader", "Deliver Supplies"),
    "Nomi": ("Play the Photophonor", "Tutoring", "Robot Repair", "Study Engineering"),
    "Rex": ("Deliver Supplies", "Construction", "Relax in the Lounge"),
    "Tammy": ("Babysitting", "Cooking", "Relax in the Lounge"),
    "Tang": ("Barista", "Study Life Sciences", "Study Humanities", "Nursing Assistant", "Study Engineering"),
    "Vace": ("Guard Duty", "Hunt in the Swamps", "Defense Training"),
    "Sym": ("Sneak Out", "Explore Nearby", "Survey the Plains", "Survey the Ridge"),
  }

  def create_regions(self) -> None:
    regions.create_and_connect_regions(self)
    locations.create_all_locations(self)

  def set_rules(self) -> None:
    rules.set_all_rules(self)

  def create_items(self) -> None:
    items.create_all_items(self)

  def create_item(self, name: str) -> items.ExocolonistItem:
    return items.create_item_with_correct_classification(self, name)

  def get_filler_item_name(self) -> str:
    return "Experience"

  def fill_slot_data(self) -> dict[str, Any]:
    return self.options.as_dict("friendsanity", "datesanity", "ending", "perksanity", "building_rando")

  @staticmethod
  def interpret_slot_data(slot_data: dict[str, Any]) -> dict[str, Any]:
    return slot_data

  def generate_early(self) -> None:
    re_gen_passthrough = getattr(self.multiworld, "re_gen_passthrough", {})
    if re_gen_passthrough and self.game in re_gen_passthrough:
      slot_data: dict[str, Any] = re_gen_passthrough[self.game]
      slot_options: dict[str, Any] = slot_data
      for key, value in slot_options.items():
        opt: Optional[Option] = getattr(self.options, key, None)
        if opt is not None:
          setattr(self.options, key, opt.from_any(value))

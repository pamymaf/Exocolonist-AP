from __future__ import annotations

from typing import TYPE_CHECKING

from BaseClasses import Item, ItemClassification

if TYPE_CHECKING:
    from .world import ExocolonistWorld

ITEM_NAME_TO_ID = {
  "Shovelling Dirt": 1,
  "Xenobotany": 2,
  "Babysitting": 3,
  "Relax in the Lounge": 4,
  "Study Life Sciences": 5,
  "Study Engineering": 6,
  "Study Humanities": 7,
  "Sportsball": 8,
  "Deliver Supplies": 9,
  "Sneak Out": 10,
  "Play the Photophonor": 11,
  "Defense Training": 12,
  "Relax on the Walls": 13,
  "Farming": 14,
  "Relax in the Park": 15,
  "Depot Clerk": 16,
  "Cooking": 17,
  "Tutoring": 18,
  "Lookout Duty": 19,
  "Explore Nearby": 20,
  "Survey the Plains": 21,
  "Forage in the Valley": 22,
  "Tending Animals": 23,
  "Robot Repair": 24,
  "Guard Duty": 25,
  "Construction": 26,
  "Administration": 27,
  "Survey the Ridge": 28,
  "Rebuild": 29,
  "Mourn": 30,
  "Barista": 31,
  "Nursing Assistant": 32,
  "Explore Glow": 33,
  "Hunt in the Swamps": 34,
  "Leader": 35,
  "Bobberfruit": 36,
  "Medicinal Roots": 37,
  "Xeno Egg": 38,
  "Yellow Flower": 39,
  "Mushwood Log": 40,
  "Crystal Cluster": 41,
  "Strange Device": 42,
  "Progressive Year": 43,
  "Experience": 44,
  "Cake": 45,
  "Progressive Empathy Perk": 46,
  "Progressive Creativity Perk": 47,
  "Progressive Persuasion Perk": 48,
  "Progressive Bravery Perk": 49,
  "Progressive Reasoning Perk": 50,
  "Progressive Engineering Perk": 51,
  "Progressive Organization Perk": 52,
  "Progressive Biology Perk": 53,
  "Progressive Toughness Perk": 54,
  "Progressive Combat Perk": 55,
  "Progressive Perception Perk": 56,
  "Progressive Animals Perk": 57,
}

ITEM_CLASSIFICATIONS = {
  "Shovelling Dirt": ItemClassification.progression,
  "Xenobotany": ItemClassification.progression,
  "Babysitting": ItemClassification.progression,
  "Relax in the Lounge": ItemClassification.progression,
  "Study Life Sciences": ItemClassification.progression,
  "Study Engineering": ItemClassification.progression,
  "Study Humanities": ItemClassification.progression,
  "Sportsball": ItemClassification.progression,
  "Deliver Supplies": ItemClassification.progression,
  "Sneak Out": ItemClassification.progression,
  "Play the Photophonor": ItemClassification.progression,
  "Defense Training": ItemClassification.progression,
  "Relax on the Walls": ItemClassification.progression,
  "Farming": ItemClassification.progression,
  "Relax in the Park": ItemClassification.progression,
  "Depot Clerk": ItemClassification.progression,
  "Cooking": ItemClassification.progression,
  "Tutoring": ItemClassification.progression,
  "Lookout Duty": ItemClassification.progression,
  "Explore Nearby": ItemClassification.progression,
  "Survey the Plains": ItemClassification.progression,
  "Forage in the Valley": ItemClassification.progression,
  "Tending Animals": ItemClassification.progression,
  "Robot Repair": ItemClassification.progression,
  "Guard Duty": ItemClassification.progression,
  "Construction": ItemClassification.progression,
  "Administration": ItemClassification.progression,
  "Survey the Ridge": ItemClassification.progression,
  "Rebuild": ItemClassification.progression,
  "Mourn": ItemClassification.progression,
  "Barista": ItemClassification.progression,
  "Nursing Assistant": ItemClassification.progression,
  "Explore Glow": ItemClassification.progression,
  "Hunt in the Swamps": ItemClassification.progression,
  "Leader": ItemClassification.progression,
  "Bobberfruit": ItemClassification.useful,
  "Medicinal Roots": ItemClassification.useful,
  "Xeno Egg": ItemClassification.useful,
  "Yellow Flower": ItemClassification.useful,
  "Mushwood Log": ItemClassification.useful,
  "Crystal Cluster": ItemClassification.useful,
  "Strange Device": ItemClassification.useful,
  "Progressive Year": ItemClassification.progression,
  "Experience": ItemClassification.filler,
  "Cake": ItemClassification.useful,
  "Progressive Empathy Perk":  ItemClassification.progression,
  "Progressive Creativity Perk":  ItemClassification.progression,
  "Progressive Persuasion Perk":  ItemClassification.progression,
  "Progressive Bravery Perk":  ItemClassification.progression,
  "Progressive Reasoning Perk":  ItemClassification.progression,
  "Progressive Engineering Perk":  ItemClassification.progression,
  "Progressive Organization Perk":  ItemClassification.progression,
  "Progressive Biology Perk":  ItemClassification.progression,
  "Progressive Toughness Perk":  ItemClassification.progression,
  "Progressive Combat Perk":  ItemClassification.progression,
  "Progressive Perception Perk":  ItemClassification.progression,
  "Progressive Animals Perk":  ItemClassification.progression,
}


class ExocolonistItem(Item):
  game = "Exocolonist"


def create_all_items(world: ExocolonistWorld) -> None:
  itempool = [
    world.create_item("Shovelling Dirt"),
    world.create_item("Xenobotany"),
    world.create_item("Babysitting"),
    world.create_item("Study Life Sciences"),
    world.create_item("Study Engineering"),
    world.create_item("Study Humanities"),
    world.create_item("Sportsball"),
    world.create_item("Deliver Supplies"),
    world.create_item("Sneak Out"),
    world.create_item("Play the Photophonor"),
    world.create_item("Defense Training"),
    world.create_item("Farming"),
    world.create_item("Depot Clerk"),
    world.create_item("Cooking"),
    world.create_item("Tutoring"),
    world.create_item("Lookout Duty"),
    world.create_item("Explore Nearby"),
    world.create_item("Survey the Plains"),
    world.create_item("Forage in the Valley"),
    world.create_item("Tending Animals"),
    world.create_item("Robot Repair"),
    world.create_item("Guard Duty"),
    world.create_item("Construction"),
    world.create_item("Administration"),
    world.create_item("Survey the Ridge"),
    world.create_item("Rebuild"),
    world.create_item("Mourn"),
    world.create_item("Barista"),
    world.create_item("Nursing Assistant"),
    world.create_item("Explore Glow"),
    world.create_item("Hunt in the Swamps"),
    world.create_item("Leader"),
    world.create_item("Progressive Year"),
    world.create_item("Progressive Year"),
    world.create_item("Progressive Year"),
    world.create_item("Progressive Year"),
    world.create_item("Progressive Year"),
    world.create_item("Progressive Year"),
    world.create_item("Progressive Year"),
    world.create_item("Progressive Year"),
    world.create_item("Progressive Year"),
    world.create_item("Progressive Year"),
  ]
  if world.options.perksanity:
    perk_items = [
      world.create_item("Progressive Empathy Perk"),
      world.create_item("Progressive Empathy Perk"),
      world.create_item("Progressive Empathy Perk"),
      world.create_item("Progressive Creativity Perk"),
      world.create_item("Progressive Creativity Perk"),
      world.create_item("Progressive Creativity Perk"),
      world.create_item("Progressive Persuasion Perk"),
      world.create_item("Progressive Persuasion Perk"),
      world.create_item("Progressive Persuasion Perk"),
      world.create_item("Progressive Bravery Perk"),
      world.create_item("Progressive Bravery Perk"),
      world.create_item("Progressive Bravery Perk"),
      world.create_item("Progressive Reasoning Perk"),
      world.create_item("Progressive Reasoning Perk"),
      world.create_item("Progressive Reasoning Perk"),
      world.create_item("Progressive Engineering Perk"),
      world.create_item("Progressive Engineering Perk"),
      world.create_item("Progressive Engineering Perk"),
      world.create_item("Progressive Organization Perk"),
      world.create_item("Progressive Organization Perk"),
      world.create_item("Progressive Organization Perk"),
      world.create_item("Progressive Biology Perk"),
      world.create_item("Progressive Biology Perk"),
      world.create_item("Progressive Biology Perk"),
      world.create_item("Progressive Toughness Perk"),
      world.create_item("Progressive Toughness Perk"),
      world.create_item("Progressive Toughness Perk"),
      world.create_item("Progressive Combat Perk"),
      world.create_item("Progressive Combat Perk"),
      world.create_item("Progressive Combat Perk"),
      world.create_item("Progressive Perception Perk"),
      world.create_item("Progressive Perception Perk"),
      world.create_item("Progressive Perception Perk"),
      world.create_item("Progressive Animals Perk"),
      world.create_item("Progressive Animals Perk"),
      world.create_item("Progressive Animals Perk"),
    ]
    itempool += perk_items

  relax_jobs = [
    world.create_item("Relax in the Lounge"),
    world.create_item("Relax on the Walls"),
    world.create_item("Relax in the Park"),
    ]
  starting_relax = world.random.choice(relax_jobs)
  world.multiworld.push_precollected(starting_relax)

  relax_jobs = [job for job in relax_jobs if job != starting_relax]
  itempool += relax_jobs


  amount_of_items = len(itempool)
  amount_of_unfilled_locations = len(world.multiworld.get_unfilled_locations(world.player))
  needed_amount_of_extra = amount_of_unfilled_locations - amount_of_items
  amount_each_item = needed_amount_of_extra // 8
  for item in ["Bobberfruit", "Medicinal Roots", "Xeno Egg", "Yellow Flower", "Mushwood Log", "Crystal Cluster", "Strange Device", "Cake"]:
    itempool += [world.create_item(item) for _ in range(amount_each_item)]
  needed_amount_of_filler = needed_amount_of_extra % 8
  itempool += [world.create_item("Experience") for _ in range(needed_amount_of_filler)]
  world.multiworld.itempool += itempool
  
    
def create_item_with_correct_classification(world: ExocolonistWorld, name: str) -> ExocolonistItem:
    classification = ITEM_CLASSIFICATIONS[name]
    return ExocolonistItem(name, classification, ITEM_NAME_TO_ID[name], world.player)

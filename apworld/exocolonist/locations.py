from __future__ import annotations

from typing import TYPE_CHECKING

from BaseClasses import ItemClassification, Location

from . import items

if TYPE_CHECKING:
  from .world import ExocolonistWorld

LOCATION_NAME_TO_ID = {
  "Shovelling Dirt": 1,
  "Farming": 2,
  "Xenobotany": 3,
  "Tending Animals": 4,
  "Relax in the Park": 5,
  "Babysitting": 6,
  "Cooking": 7,
  "Barista": 8,
  "Play the Photophonor": 9,
  "Relax in the Lounge": 10,
  "Study Life Sciences": 11,
  "Study Engineering": 12,
  "Study Humanities": 13,
  "Tutoring": 14,
  "Robot Repair": 15,
  "Nursing Assistant": 16,
  "Sportsball": 17,
  "Defense Training": 18,
  "Lookout Duty": 19,
  "Guard Duty": 20,
  "Relax on the Walls": 21,
  "Deliver Supplies": 22,
  "Depot Clerk": 23,
  "Construction": 24,
  "Administration": 25,
  "Leader": 26,
  "Sneak Out": 27,
  "Explore Nearby": 28,
  "Survey the Plains": 29,
  "Forage in the Valley": 30,
  "Survey the Ridge": 31,
  "Hunt in the Swamps": 32,
  "Explore Glow": 33,
  "Rebuild": 34,
  "Mourn": 35,
  # Add space between categories just in case
  "Ending": 56,
  "Bobberfruit": 57,
  "Medicinal Roots": 58,
  "Xeno Egg": 59,
  "Yellow Flower": 60,
  "Mushwood Log": 61,
  "Crystal Cluster": 62,
  "Strange Device": 63,
  "Cake": 64,
  # Add space between categories just in case
  "Save Tammy": 154,
  "Save Tonin": 156,
  "Save Mom": 157,
  "Save Dad": 158,
  "Save Eudicot": 159,
  "Save Hal": 160,
  # Add space between categories just in case
  "Adopt Vriki": 180,
  "Adopt Hopeye": 181,
  "Adopt Robot": 182,
  "Adopt Unisaur": 183,
  "Become Governor": 184,
  "Marz Governor": 185,
  # Add space between categories just in case
  "Empathy Perk 1": 200,
  "Empathy Perk 2": 201,
  "Empathy Perk 3": 202,
  "Creativity Perk 1": 203,
  "Creativity Perk 2": 204,
  "Creativity Perk 3": 205,
  "Persuasion Perk 1": 206,
  "Persuasion Perk 2": 207,
  "Persuasion Perk 3": 208,
  "Bravery Perk 1": 209,
  "Bravery Perk 2": 210,
  "Bravery Perk 3": 211,
  "Engineering Perk 1": 212,
  "Engineering Perk 2": 213,
  "Engineering Perk 3": 214,
  "Reasoning Perk 1": 215,
  "Reasoning Perk 2": 216,
  "Reasoning Perk 3": 217,
  "Organization Perk 1": 218,
  "Organization Perk 2": 219,
  "Organization Perk 3": 220,
  "Biology Perk 1": 221,
  "Biology Perk 2": 222,
  "Biology Perk 3": 223,
  "Toughness Perk 1": 224,
  "Toughness Perk 2": 225,
  "Toughness Perk 3": 226,
  "Combat Perk 1": 227,
  "Combat Perk 2": 228,
  "Combat Perk 3": 229,
  "Perception Perk 1": 230,
  "Perception Perk 2": 231,
  "Perception Perk 3": 232,
  "Animals Perk 1": 233,
  "Animals Perk 2": 234,
  "Animals Perk 3": 235,
  # Lots of space before this category
  "Anemone 10": 500,
  "Anemone 100": 509,
  "Anemone 20": 501,
  "Anemone 30": 502,
  "Anemone 40": 503,
  "Anemone 50": 504,
  "Anemone 60": 505,
  "Anemone 70": 506,
  "Anemone 80": 507,
  "Anemone 90": 508,
  "Date Anemone": 510,
  "Cal 10": 511,
  "Cal 100": 520,
  "Cal 20": 512,
  "Cal 30": 513,
  "Cal 40": 514,
  "Cal 50": 515,
  "Cal 60": 516,
  "Cal 70": 517,
  "Cal 80": 518,
  "Cal 90": 519,
  "Date Cal": 521,
  "Dys 10": 555,
  "Dys 100": 564,
  "Dys 20": 556,
  "Dys 30": 557,
  "Dys 40": 558,
  "Dys 50": 559,
  "Dys 60": 560,
  "Dys 70": 561,
  "Dys 80": 562,
  "Dys 90": 563,
  "Date Dys": 565,
  "Marz 10": 522,
  "Marz 100": 531,
  "Marz 20": 523,
  "Marz 30": 524,
  "Marz 40": 525,
  "Marz 50": 526,
  "Marz 60": 527,
  "Marz 70": 528,
  "Marz 80": 529,
  "Marz 90": 530,
  "Date Marz": 532,
  "Nomi 10": 577,
  "Nomi 100": 586,
  "Nomi 20": 578,
  "Nomi 30": 579,
  "Nomi 40": 580,
  "Nomi 50": 581,
  "Nomi 60": 582,
  "Nomi 70": 583,
  "Nomi 80": 584,
  "Nomi 90": 585,
  "Date Nomi": 587,
  "Rex 10": 588,
  "Rex 100": 597,
  "Rex 20": 589,
  "Rex 30": 590,
  "Rex 40": 591,
  "Rex 50": 592,
  "Rex 60": 593,
  "Rex 70": 594,
  "Rex 80": 595,
  "Rex 90": 596,
  "Date Rex": 598,
  "Sym 10": 566,
  "Sym 100": 575,
  "Sym 20": 567,
  "Sym 30": 568,
  "Sym 40": 569,
  "Sym 50": 570,
  "Sym 60": 571,
  "Sym 70": 572,
  "Sym 80": 573,
  "Sym 90": 574,
  "Date Sym": 576,
  "Tammy 10": 533,
  "Tammy 100": 542,
  "Tammy 20": 534,
  "Tammy 30": 535,
  "Tammy 40": 536,
  "Tammy 50": 537,
  "Tammy 60": 538,
  "Tammy 70": 539,
  "Tammy 80": 540,
  "Tammy 90": 541,
  "Date Tammy": 543,
  "Tang 10": 544,
  "Tang 100": 553,
  "Tang 20": 545,
  "Tang 30": 546,
  "Tang 40": 547,
  "Tang 50": 548,
  "Tang 60": 549,
  "Tang 70": 550,
  "Tang 80": 551,
  "Tang 90": 552,
  "Date Tang": 554,
  "Vace 10": 599,
  "Vace 100": 608,
  "Vace 20": 600,
  "Vace 30": 601,
  "Vace 40": 602,
  "Vace 50": 603,
  "Vace 60": 604,
  "Vace 70": 605,
  "Vace 80": 606,
  "Vace 90": 607,
  "Date Vace": 609
}


class ExocolonistLocation(Location):
  game = "Exocolonist"


def create_all_locations(world: ExocolonistWorld) -> None:
  create_regular_locations(world)
  create_events(world)


def create_regular_locations(world: ExocolonistWorld) -> None:
  start = world.get_region("Start")
  age10 = world.get_region("Age 10")
  age11 = world.get_region("Age 11")
  age12 = world.get_region("Age 12")
  age13 = world.get_region("Age 13")
  age14 = world.get_region("Age 14")
  age15 = world.get_region("Age 15")
  age16 = world.get_region("Age 16")
  age17 = world.get_region("Age 17")
  age18 = world.get_region("Age 18")
  age19 = world.get_region("Age 19")
  age20 = world.get_region("Age 20")

  start_locations: dict[str, int | None] = {
    "Relax in the Lounge": world.location_name_to_id["Relax in the Lounge"],
    "Study Life Sciences": world.location_name_to_id["Study Life Sciences"],
    "Study Humanities": world.location_name_to_id["Study Humanities"],
  }
  start.add_locations(start_locations, ExocolonistLocation)

  age10_locations: dict[str, int | None] = {
    "Shovelling Dirt": world.location_name_to_id["Shovelling Dirt"],
    "Xenobotany": world.location_name_to_id["Xenobotany"],
    "Babysitting": world.location_name_to_id["Babysitting"],
    "Study Engineering": world.location_name_to_id["Study Engineering"],
    "Sportsball": world.location_name_to_id["Sportsball"],
    "Deliver Supplies": world.location_name_to_id["Deliver Supplies"],
    "Sneak Out": world.location_name_to_id["Sneak Out"],
    "Save Tammy": world.location_name_to_id["Save Tammy"],
    "Bobberfruit": world.location_name_to_id["Bobberfruit"],
    "Medicinal Roots": world.location_name_to_id["Medicinal Roots"],
    "Xeno Egg": world.location_name_to_id["Xeno Egg"],
    "Yellow Flower": world.location_name_to_id["Yellow Flower"],
    "Mushwood Log": world.location_name_to_id["Mushwood Log"],
    "Crystal Cluster": world.location_name_to_id["Crystal Cluster"],
    "Strange Device": world.location_name_to_id["Strange Device"],
    "Adopt Robot": world.location_name_to_id["Adopt Robot"],
  }
  age10.add_locations(age10_locations, ExocolonistLocation)

  age11_locations: dict[str, int | None] = {
    "Defense Training": world.location_name_to_id["Defense Training"],
    "Relax on the Walls": world.location_name_to_id["Relax on the Walls"],
    "Save Tonin": world.location_name_to_id["Save Tonin"],
    "Save Hal": world.location_name_to_id["Save Hal"],
    "Adopt Hopeye": world.location_name_to_id["Adopt Hopeye"],
  }
  age11.add_locations(age11_locations, ExocolonistLocation)

  age12_locations: dict[str, int | None] = {
    "Play the Photophonor": world.location_name_to_id["Play the Photophonor"],
    "Farming": world.location_name_to_id["Farming"],
    "Relax in the Park": world.location_name_to_id["Relax in the Park"],
    "Depot Clerk": world.location_name_to_id["Depot Clerk"],
    "Adopt Vriki": world.location_name_to_id["Adopt Vriki"],
    "Adopt Unisaur": world.location_name_to_id["Adopt Unisaur"],
  }
  age12.add_locations(age12_locations, ExocolonistLocation)

  age13_locations: dict[str, int | None] = {
    "Cooking": world.location_name_to_id["Cooking"],
    "Tutoring": world.location_name_to_id["Tutoring"],
    "Lookout Duty": world.location_name_to_id["Lookout Duty"],
    "Explore Nearby": world.location_name_to_id["Explore Nearby"],
    "Survey the Plains": world.location_name_to_id["Survey the Plains"],
    "Forage in the Valley": world.location_name_to_id["Forage in the Valley"],
  }
  age13.add_locations(age13_locations, ExocolonistLocation)

  age14_locations: dict[str, int | None] = {
    "Tending Animals": world.location_name_to_id["Tending Animals"],
    "Save Mom": world.location_name_to_id["Save Mom"],
    "Save Eudicot": world.location_name_to_id["Save Eudicot"],
  }
  age14.add_locations(age14_locations, ExocolonistLocation)

  age15_locations: dict[str, int | None] = {
    "Robot Repair": world.location_name_to_id["Robot Repair"],
    "Guard Duty": world.location_name_to_id["Guard Duty"],
    "Construction": world.location_name_to_id["Construction"],
    "Administration": world.location_name_to_id["Administration"],
    "Survey the Ridge": world.location_name_to_id["Survey the Ridge"],
    "Rebuild": world.location_name_to_id["Rebuild"],
    "Mourn": world.location_name_to_id["Mourn"],
    "Save Dad": world.location_name_to_id["Save Dad"],
  }
  age15.add_locations(age15_locations, ExocolonistLocation)

  age16_locations: dict[str, int | None] = {
    "Barista": world.location_name_to_id["Barista"],
    "Nursing Assistant": world.location_name_to_id["Nursing Assistant"],
    "Explore Glow": world.location_name_to_id["Explore Glow"],

  }
  age16.add_locations(age16_locations, ExocolonistLocation)

  age17_locations: dict[str, int | None] = {
    "Hunt in the Swamps": world.location_name_to_id["Hunt in the Swamps"],
  }
  age17.add_locations(age17_locations, ExocolonistLocation)

  age18_locations: dict[str, int | None] = {

  }
  age18.add_locations(age18_locations, ExocolonistLocation)

  age19_locations: dict[str, int | None] = {
    "Leader": world.location_name_to_id["Leader"],
    "Become Governor": world.location_name_to_id["Become Governor"],
    "Marz Governor": world.location_name_to_id["Marz Governor"],
  }
  age19.add_locations(age19_locations, ExocolonistLocation)

  age20_locations: dict[str, int | None] = {
    
  }
  age20.add_locations(age20_locations, ExocolonistLocation)

  if world.options.friendsanity:
    age10_friends: dict[str, int | None] = {
      "Marz 10": world.location_name_to_id["Marz 10"],
      "Dys 10": world.location_name_to_id["Dys 10"],
      "Cal 10": world.location_name_to_id["Cal 10"],
      "Tammy 10": world.location_name_to_id["Tammy 10"],
      "Tang 10": world.location_name_to_id["Tang 10"],
      "Anemone 10": world.location_name_to_id["Anemone 10"],
      "Marz 20": world.location_name_to_id["Marz 20"],
      "Dys 20": world.location_name_to_id["Dys 20"],
      "Cal 20": world.location_name_to_id["Cal 20"],
      "Tammy 20": world.location_name_to_id["Tammy 20"],
      "Tang 20": world.location_name_to_id["Tang 20"],
      "Anemone 20": world.location_name_to_id["Anemone 20"],
    }
    age10.add_locations(age10_friends, ExocolonistLocation)

    age11_friends: dict[str, int | None] = {
      "Marz 30": world.location_name_to_id["Marz 30"],
      "Dys 30": world.location_name_to_id["Dys 30"],
      "Cal 30": world.location_name_to_id["Cal 30"],
      "Tammy 30": world.location_name_to_id["Tammy 30"],
      "Tang 30": world.location_name_to_id["Tang 30"],
      "Anemone 30": world.location_name_to_id["Anemone 30"],
      "Marz 40": world.location_name_to_id["Marz 40"],
      "Dys 40": world.location_name_to_id["Dys 40"],
      "Cal 40": world.location_name_to_id["Cal 40"],
      "Tammy 40": world.location_name_to_id["Tammy 40"],
      "Tang 40": world.location_name_to_id["Tang 40"],
      "Anemone 40": world.location_name_to_id["Anemone 40"],
    }
    age11.add_locations(age11_friends, ExocolonistLocation)

    age12_friends: dict[str, int | None] = {
      "Marz 50": world.location_name_to_id["Marz 50"],
      "Dys 50": world.location_name_to_id["Dys 50"],
      "Cal 50": world.location_name_to_id["Cal 50"],
      "Tammy 50": world.location_name_to_id["Tammy 50"],
      "Tang 50": world.location_name_to_id["Tang 50"],
      "Anemone 50": world.location_name_to_id["Anemone 50"],
      "Marz 60": world.location_name_to_id["Marz 60"],
      "Dys 60": world.location_name_to_id["Dys 60"],
      "Cal 60": world.location_name_to_id["Cal 60"],
      "Tammy 60": world.location_name_to_id["Tammy 60"],
      "Tang 60": world.location_name_to_id["Tang 60"],
      "Anemone 60": world.location_name_to_id["Anemone 60"],
    }
    age12.add_locations(age12_friends, ExocolonistLocation)

    age13_friends: dict[str, int | None] = {
      "Marz 70": world.location_name_to_id["Marz 70"],
      "Dys 70": world.location_name_to_id["Dys 70"],
      "Cal 70": world.location_name_to_id["Cal 70"],
      "Tammy 70": world.location_name_to_id["Tammy 70"],
      "Tang 70": world.location_name_to_id["Tang 70"],
      "Anemone 70": world.location_name_to_id["Anemone 70"],
      "Marz 80": world.location_name_to_id["Marz 80"],
      "Dys 80": world.location_name_to_id["Dys 80"],
      "Cal 80": world.location_name_to_id["Cal 80"],
      "Tammy 80": world.location_name_to_id["Tammy 80"],
      "Tang 80": world.location_name_to_id["Tang 80"],
      "Anemone 80": world.location_name_to_id["Anemone 80"],
    }
    age13.add_locations(age13_friends, ExocolonistLocation)

    age14_friends: dict[str, int | None] = {
      "Marz 90": world.location_name_to_id["Marz 90"],
      "Dys 90": world.location_name_to_id["Dys 90"],
      "Cal 90": world.location_name_to_id["Cal 90"],
      "Tammy 90": world.location_name_to_id["Tammy 90"],
      "Tang 90": world.location_name_to_id["Tang 90"],
      "Anemone 90": world.location_name_to_id["Anemone 90"],
      "Marz 100": world.location_name_to_id["Marz 100"],
      "Dys 100": world.location_name_to_id["Dys 100"],
      "Cal 100": world.location_name_to_id["Cal 100"],
      "Tammy 100": world.location_name_to_id["Tammy 100"],
      "Tang 100": world.location_name_to_id["Tang 100"],
      "Anemone 100": world.location_name_to_id["Anemone 100"],
    }
    age14.add_locations(age14_friends, ExocolonistLocation)

    age15_friends: dict[str, int | None] = {
      "Rex 10": world.location_name_to_id["Rex 10"],
      "Nomi 10": world.location_name_to_id["Nomi 10"],
      "Vace 10": world.location_name_to_id["Vace 10"],
      "Sym 10": world.location_name_to_id["Sym 10"],
      "Rex 20": world.location_name_to_id["Rex 20"],
      "Nomi 20": world.location_name_to_id["Nomi 20"],
      "Vace 20": world.location_name_to_id["Vace 20"],
      "Sym 20": world.location_name_to_id["Sym 20"],
    }
    age15.add_locations(age15_friends, ExocolonistLocation)

    age16_friends: dict[str, int | None] = {
      "Rex 30": world.location_name_to_id["Rex 30"],
      "Nomi 30": world.location_name_to_id["Nomi 30"],
      "Vace 30": world.location_name_to_id["Vace 30"],
      "Sym 30": world.location_name_to_id["Sym 30"],
      "Rex 40": world.location_name_to_id["Rex 40"],
      "Nomi 40": world.location_name_to_id["Nomi 40"],
      "Vace 40": world.location_name_to_id["Vace 40"],
      "Sym 40": world.location_name_to_id["Sym 40"],
    }
    age16.add_locations(age16_friends, ExocolonistLocation)

    age16_friends: dict[str, int | None] = {
      "Rex 50": world.location_name_to_id["Rex 50"],
      "Nomi 50": world.location_name_to_id["Nomi 50"],
      "Vace 50": world.location_name_to_id["Vace 50"],
      "Sym 50": world.location_name_to_id["Sym 50"],
      "Rex 60": world.location_name_to_id["Rex 60"],
      "Nomi 60": world.location_name_to_id["Nomi 60"],
      "Vace 60": world.location_name_to_id["Vace 60"],
      "Sym 60": world.location_name_to_id["Sym 60"],
    }
    age16.add_locations(age16_friends, ExocolonistLocation)

    age17_friends: dict[str, int | None] = {
      "Rex 70": world.location_name_to_id["Rex 70"],
      "Nomi 70": world.location_name_to_id["Nomi 70"],
      "Vace 70": world.location_name_to_id["Vace 70"],
      "Sym 70": world.location_name_to_id["Sym 70"],
      "Rex 80": world.location_name_to_id["Rex 80"],
      "Nomi 80": world.location_name_to_id["Nomi 80"],
      "Vace 80": world.location_name_to_id["Vace 80"],
      "Sym 80": world.location_name_to_id["Sym 80"],
    }
    age17.add_locations(age17_friends, ExocolonistLocation)

    age18_friends: dict[str, int | None] = {
      "Rex 90": world.location_name_to_id["Rex 90"],
      "Nomi 90": world.location_name_to_id["Nomi 90"],
      "Vace 90": world.location_name_to_id["Vace 90"],
      "Sym 90": world.location_name_to_id["Sym 90"],
      "Rex 100": world.location_name_to_id["Rex 100"],
      "Nomi 100": world.location_name_to_id["Nomi 100"],
      "Vace 100": world.location_name_to_id["Vace 100"],
      "Sym 100": world.location_name_to_id["Sym 100"],
    }
    age18.add_locations(age18_friends, ExocolonistLocation)
  
  if world.options.datesanity:
    age16_dates: dict[str, int | None] = {
      "Date Marz": world.location_name_to_id["Date Marz"],
      "Date Dys": world.location_name_to_id["Date Dys"],
      "Date Cal": world.location_name_to_id["Date Cal"],
      "Date Tammy": world.location_name_to_id["Date Tammy"],
      "Date Tang": world.location_name_to_id["Date Tang"],
      "Date Anemone": world.location_name_to_id["Date Anemone"],
    }
    age16.add_locations(age16_dates, ExocolonistLocation)

    age19_dates: dict[str, int | None] = {
      "Date Rex": world.location_name_to_id["Date Rex"],
      "Date Nomi": world.location_name_to_id["Date Nomi"],
      "Date Vace": world.location_name_to_id["Date Vace"],
      "Date Sym": world.location_name_to_id["Date Sym"],
    }
    age19.add_locations(age19_dates, ExocolonistLocation)
  
  if world.options.perksanity:
    age11_perk_locations: dict[str, int | None] = {
      "Creativity Perk 1": world.location_name_to_id["Creativity Perk 1"],
      "Persuasion Perk 1": world.location_name_to_id["Persuasion Perk 1"],
      "Empathy Perk 1": world.location_name_to_id["Empathy Perk 1"],
      "Bravery Perk 1": world.location_name_to_id["Bravery Perk 1"],
      "Reasoning Perk 1": world.location_name_to_id["Reasoning Perk 1"],
      "Engineering Perk 1": world.location_name_to_id["Engineering Perk 1"],
      "Organization Perk 1": world.location_name_to_id["Organization Perk 1"],
      "Biology Perk 1": world.location_name_to_id["Biology Perk 1"],
      "Combat Perk 1": world.location_name_to_id["Combat Perk 1"],
      "Toughness Perk 1": world.location_name_to_id["Toughness Perk 1"],
      "Perception Perk 1": world.location_name_to_id["Perception Perk 1"],
      "Animals Perk 1": world.location_name_to_id["Animals Perk 1"],
    }
    age11.add_locations(age11_perk_locations, ExocolonistLocation)

    age13_perk_locations: dict[str, int | None] = {
      "Creativity Perk 2": world.location_name_to_id["Creativity Perk 2"],
      "Persuasion Perk 2": world.location_name_to_id["Persuasion Perk 2"],
      "Empathy Perk 2": world.location_name_to_id["Empathy Perk 2"],
      "Bravery Perk 2": world.location_name_to_id["Bravery Perk 2"],
      "Reasoning Perk 2": world.location_name_to_id["Reasoning Perk 2"],
      "Engineering Perk 2": world.location_name_to_id["Engineering Perk 2"],
      "Organization Perk 2": world.location_name_to_id["Organization Perk 2"],
      "Biology Perk 2": world.location_name_to_id["Biology Perk 2"],
      "Combat Perk 2": world.location_name_to_id["Combat Perk 2"],
      "Toughness Perk 2": world.location_name_to_id["Toughness Perk 2"],
      "Perception Perk 2": world.location_name_to_id["Perception Perk 2"],
      "Animals Perk 2": world.location_name_to_id["Animals Perk 2"],
    }
    age13.add_locations(age13_perk_locations, ExocolonistLocation)

    age15_perk_locations: dict[str, int | None] = {
      "Creativity Perk 3": world.location_name_to_id["Creativity Perk 3"],
      "Persuasion Perk 3": world.location_name_to_id["Persuasion Perk 3"],
      "Empathy Perk 3": world.location_name_to_id["Empathy Perk 3"],
      "Bravery Perk 3": world.location_name_to_id["Bravery Perk 3"],
      "Reasoning Perk 3": world.location_name_to_id["Reasoning Perk 3"],
      "Engineering Perk 3": world.location_name_to_id["Engineering Perk 3"],
      "Organization Perk 3": world.location_name_to_id["Organization Perk 3"],
      "Biology Perk 3": world.location_name_to_id["Biology Perk 3"],
      "Combat Perk 3": world.location_name_to_id["Combat Perk 3"],
      "Toughness Perk 3": world.location_name_to_id["Toughness Perk 3"],
      "Perception Perk 3": world.location_name_to_id["Perception Perk 3"],
      "Animals Perk 3": world.location_name_to_id["Animals Perk 3"],
    }
    age15.add_locations(age15_perk_locations, ExocolonistLocation)

def create_events(world: ExocolonistWorld) -> None:
  age20 = world.get_region("Age 20")
  age20.add_event(
    "Ending", "Ending", location_type=ExocolonistLocation, item_type=items.ExocolonistItem
  )
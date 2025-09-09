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
    "Ending": 36,
    "Bobberfruit": 37,
    "Medicinal Root": 38,
    "Xeno Egg": 39,
    "Yellow Flower": 40,
    "Mushwood Log": 41,
    "Crystal Cluster": 42,
    "Strange Device": 43,
    "Mars 20": 44,
    "Mars 40": 45,
    "Mars 60": 46,
    "Mars 80": 47,
    "Mars 100": 48,
    "Mars Date": 49,
    "Dys 20": 50,
    "Dys 40": 51,
    "Dys 60": 52,
    "Dys 80": 53,
    "Dys 100": 54,
    "Dys Date": 55,
    "Cal 20": 56,
    "Cal 40": 57,
    "Cal 60": 58,
    "Cal 80": 59,
    "Cal 100": 60,
    "Cal Date": 61,
    "Tammy 20": 62,
    "Tammy 40": 63,
    "Tammy 60": 64,
    "Tammy 80": 65,
    "Tammy 100": 66,
    "Tammy Date": 67,
    "Anemone 20": 68,
    "Anemone 40": 69,
    "Anemone 60": 70,
    "Anemone 80": 71,
    "Anemone 100": 72,
    "Anemone Date": 73,
    "Rex 20": 74,
    "Rex 40": 75,
    "Rex 60": 76,
    "Rex 80": 77,
    "Rex 100": 78,
    "Rex Date": 79,
    "Nomi 20": 80,
    "Nomi 40": 81,
    "Nomi 60": 82,
    "Nomi 80": 83,
    "Nomi 100": 84,
    "Nomi Date": 85,
    "Vace 20": 86,
    "Vace 40": 87,
    "Vace 60": 88,
    "Vace 80": 89,
    "Vace 100": 90,
    "Vace Date": 91,
    "Sym 20": 92,
    "Sym 40": 93,
    "Sym 60": 94,
    "Sym 80": 95,
    "Sym 100": 96,
    "Sym Date": 97,
    "Save Tammy": 98,
    "Rescue Tammy": 99,
    "Rescue Tonin": 100,
    "Save Mom": 101,
    "Save Dad": 102,
    "Rescue Eudicot": 103,
    "Adopt Vriki": 104,
    "Adopt Hopeye": 105,
    "Adopt Robot": 106,
    "Adopt Unisaur": 107,
    "Become Governor": 108,
    "Mars Governor": 109,

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
        "Mars 20": world.location_name_to_id["Mars 20"],
        "Dys 20": world.location_name_to_id["Dys 20"],
        "Cal 20": world.location_name_to_id["Cal 20"],
        "Tammy 20": world.location_name_to_id["Tammy 20"],
        "Anemone 20": world.location_name_to_id["Anemone 20"],
        "Save Tammy": world.location_name_to_id["Save Tammy"],
        "Bobberfruit": world.location_name_to_id["Bobberfruit"],
        "Medicinal Root": world.location_name_to_id["Medicinal Root"],
        "Xeno Egg": world.location_name_to_id["Xeno Egg"],
        "Yellow Flower": world.location_name_to_id["Yellow Flower"],
        "Mushwood Log": world.location_name_to_id["Mushwood Log"],
        "Crystal Cluster": world.location_name_to_id["Crystal Cluster"],
        "Strange Device": world.location_name_to_id["Strange Device"],
    }
    age10.add_locations(age10_locations, ExocolonistLocation)

    age11_locations: dict[str, int | None] = {
        "Play the Photophonor": world.location_name_to_id["Play the Photophonor"],
        "Defense Training": world.location_name_to_id["Defense Training"],
        "Relax on the Walls": world.location_name_to_id["Relax on the Walls"],
        "Mars 40": world.location_name_to_id["Mars 40"],
        "Dys 40": world.location_name_to_id["Dys 40"],
        "Cal 40": world.location_name_to_id["Cal 40"],
        "Tammy 40": world.location_name_to_id["Tammy 40"],
        "Anemone 40": world.location_name_to_id["Anemone 40"],
        "Rescue Tonin": world.location_name_to_id["Rescue Tonin"],
    }
    age11.add_locations(age11_locations, ExocolonistLocation)

    age12_locations: dict[str, int | None] = {
        "Farming": world.location_name_to_id["Farming"],
        "Relax in the Park": world.location_name_to_id["Relax in the Park"],
        "Depot Clerk": world.location_name_to_id["Depot Clerk"],
        "Mars 60": world.location_name_to_id["Mars 60"],
        "Dys 60": world.location_name_to_id["Dys 60"],
        "Cal 60": world.location_name_to_id["Cal 60"],
        "Tammy 60": world.location_name_to_id["Tammy 60"],
        "Anemone 60": world.location_name_to_id["Anemone 60"],
    }
    age12.add_locations(age12_locations, ExocolonistLocation)

    age13_locations: dict[str, int | None] = {
        "Cooking": world.location_name_to_id["Cooking"],
        "Tutoring": world.location_name_to_id["Tutoring"],
        "Lookout Duty": world.location_name_to_id["Lookout Duty"],
        "Explore Nearby": world.location_name_to_id["Explore Nearby"],
        "Survey the Plains": world.location_name_to_id["Survey the Plains"],
        "Forage in the Valley": world.location_name_to_id["Forage in the Valley"],
        "Mars 80": world.location_name_to_id["Mars 80"],
        "Dys 80": world.location_name_to_id["Dys 80"],
        "Cal 80": world.location_name_to_id["Cal 80"],
        "Tammy 80": world.location_name_to_id["Tammy 80"],
        "Anemone 80": world.location_name_to_id["Anemone 80"],
    }
    age13.add_locations(age13_locations, ExocolonistLocation)

    age14_locations: dict[str, int | None] = {
        "Tending Animals": world.location_name_to_id["Tending Animals"],
        "Mars 100": world.location_name_to_id["Mars 100"],
        "Dys 100": world.location_name_to_id["Dys 100"],
        "Cal 100": world.location_name_to_id["Cal 100"],
        "Tammy 100": world.location_name_to_id["Tammy 100"],
        "Anemone 100": world.location_name_to_id["Anemone 100"],
        "Save Mom": world.location_name_to_id["Save Mom"],
        "Rescue Eudicot": world.location_name_to_id["Rescue Eudicot"],
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
        "Mars Date": world.location_name_to_id["Mars Date"],
        "Dys Date": world.location_name_to_id["Dys Date"],
        "Cal Date": world.location_name_to_id["Cal Date"],
        "Tammy Date": world.location_name_to_id["Tammy Date"],
        "Anemone Date": world.location_name_to_id["Anemone Date"],
        "Rex 20": world.location_name_to_id["Rex 20"],
        "Nomi 20": world.location_name_to_id["Nomi 20"],
        "Vace 20": world.location_name_to_id["Vace 20"],
        "Sym 20": world.location_name_to_id["Sym 20"],
        "Save Dad": world.location_name_to_id["Save Dad"],
    }
    age15.add_locations(age15_locations, ExocolonistLocation)

    age16_locations: dict[str, int | None] = {
        "Barista": world.location_name_to_id["Barista"],
        "Nursing Assistant": world.location_name_to_id["Nursing Assistant"],
        "Explore Glow": world.location_name_to_id["Explore Glow"],
        "Rex 40": world.location_name_to_id["Rex 40"],
        "Nomi 40": world.location_name_to_id["Nomi 40"],
        "Vace 40": world.location_name_to_id["Vace 40"],
        "Sym 40": world.location_name_to_id["Sym 40"],

    }
    age16.add_locations(age16_locations, ExocolonistLocation)

    age17_locations: dict[str, int | None] = {
        "Hunt in the Swamps": world.location_name_to_id["Hunt in the Swamps"],
        "Rex 60": world.location_name_to_id["Rex 60"],
        "Nomi 60": world.location_name_to_id["Nomi 60"],
        "Vace 60": world.location_name_to_id["Vace 60"],
        "Sym 60": world.location_name_to_id["Sym 60"],
    }
    age17.add_locations(age17_locations, ExocolonistLocation)

    age18_locations: dict[str, int | None] = {
        "Rex 80": world.location_name_to_id["Rex 80"],
        "Nomi 80": world.location_name_to_id["Nomi 80"],
        "Vace 80": world.location_name_to_id["Vace 80"],
        "Sym 80": world.location_name_to_id["Sym 80"],
    }
    age18.add_locations(age18_locations, ExocolonistLocation)

    age19_locations: dict[str, int | None] = {
        "Leader": world.location_name_to_id["Leader"],
        "Rex 100": world.location_name_to_id["Rex 100"],
        "Nomi 100": world.location_name_to_id["Nomi 100"],
        "Vace 100": world.location_name_to_id["Vace 100"],
        "Sym 100": world.location_name_to_id["Sym 100"],
        "Rex Date": world.location_name_to_id["Rex Date"],
        "Nomi Date": world.location_name_to_id["Nomi Date"],
        "Vace Date": world.location_name_to_id["Vace Date"],
        "Sym Date": world.location_name_to_id["Sym Date"],
    }
    age19.add_locations(age19_locations, ExocolonistLocation)

    age20_locations: dict[str, int | None] = {
        
    }
    age20.add_locations(age20_locations, ExocolonistLocation)
    

def create_events(world: ExocolonistWorld) -> None:
    age20 = world.get_region("Age 20")
    age20.add_event(
        "Ending", "Ending", location_type=ExocolonistLocation, item_type=items.ExocolonistItem
    )
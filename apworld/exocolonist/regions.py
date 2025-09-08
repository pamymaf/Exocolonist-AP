from __future__ import annotations

from typing import TYPE_CHECKING

from BaseClasses import Entrance, Region

if TYPE_CHECKING:
    from .world import ExocolonistWorld


def create_and_connect_regions(world: ExocolonistWorld) -> None:
    create_all_regions(world)
    connect_regions(world)


def create_all_regions(world: ExocolonistWorld) -> None:
    age10 = Region("Age 10", world.player, world.multiworld)
    age11 = Region("Age 11", world.player, world.multiworld)
    age12 = Region("Age 12", world.player, world.multiworld)
    age13 = Region("Age 13", world.player, world.multiworld)
    age14 = Region("Age 14", world.player, world.multiworld)
    age15 = Region("Age 15", world.player, world.multiworld)
    age16 = Region("Age 16", world.player, world.multiworld)
    age17 = Region("Age 17", world.player, world.multiworld)
    age18 = Region("Age 18", world.player, world.multiworld)
    age19 = Region("Age 19", world.player, world.multiworld)
    age20 = Region("Age 20", world.player, world.multiworld)
    
    regions = [age10, age11, age12, age13, age14, age15, age16, age17, age18, age19, age20]

    world.multiworld.regions += regions


def connect_regions(world: ExocolonistWorld) -> None:
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

    age10.connect(age11, "Aged to 11")
    age11.connect(age12, "Aged to 12")
    age12.connect(age13, "Aged to 13")
    age13.connect(age14, "Aged to 14")
    age14.connect(age15, "Aged to 15")
    age15.connect(age16, "Aged to 16")
    age16.connect(age17, "Aged to 17")
    age17.connect(age18, "Aged to 18")
    age18.connect(age19, "Aged to 19")
    age19.connect(age20, "Aged to 20")

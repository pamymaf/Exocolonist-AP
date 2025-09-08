from __future__ import annotations

from typing import TYPE_CHECKING

from BaseClasses import CollectionState

from ..generic.Rules import add_rule, set_rule

if TYPE_CHECKING:
    from .world import ExocolonistWorld


def set_all_rules(world: ExocolonistWorld) -> None:
    set_all_entrance_rules(world)
    set_all_location_rules(world)
    set_victory_condition(world)


def set_all_entrance_rules(world: ExocolonistWorld) -> None:
    age10_to_age11 = world.get_entrance("Aged to 11")
    age11_to_age12 = world.get_entrance("Aged to 12")
    age12_to_age13 = world.get_entrance("Aged to 13")
    age13_to_age14 = world.get_entrance("Aged to 14")
    age14_to_age15 = world.get_entrance("Aged to 15")
    age15_to_age16 = world.get_entrance("Aged to 16")
    age16_to_age17 = world.get_entrance("Aged to 17")
    age17_to_age18 = world.get_entrance("Aged to 18")
    age18_to_age19 = world.get_entrance("Aged to 19")
    age19_to_age20 = world.get_entrance("Aged to 20")

    set_rule(age10_to_age11, lambda state: state.has("Progressive Year", world.player, 1))
    set_rule(age11_to_age12, lambda state: state.has("Progressive Year", world.player, 2))
    set_rule(age12_to_age13, lambda state: state.has("Progressive Year", world.player, 3))
    set_rule(age13_to_age14, lambda state: state.has("Progressive Year", world.player, 4))
    set_rule(age14_to_age15, lambda state: state.has("Progressive Year", world.player, 5))
    set_rule(age15_to_age16, lambda state: state.has("Progressive Year", world.player, 6))
    set_rule(age16_to_age17, lambda state: state.has("Progressive Year", world.player, 7))
    set_rule(age17_to_age18, lambda state: state.has("Progressive Year", world.player, 8))
    set_rule(age18_to_age19, lambda state: state.has("Progressive Year", world.player, 9))
    set_rule(age19_to_age20, lambda state: state.has("Progressive Year", world.player, 10))

def set_all_location_rules(world: ExocolonistWorld) -> None:
  pass



def set_victory_condition(world: ExocolonistWorld) -> None:
    world.multiworld.completion_condition[world.player] = lambda state: state.has("Ending", world.player)

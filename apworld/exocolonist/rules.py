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
    start_to_age10 = world.get_entrance("Aged to 10")
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
    set_character_location_rules(world)
    set_consumable_rules(world)
    set_job_rules(world)


def set_consumable_rules(world: ExocolonistWorld) -> None:
    # Explore the Ridge and Explore Glow are Special
    explore_jobs = ("Sneak Out", "Explore Nearby", "Survey the Plains", "Forage in the Valley")
    
    # First year is xeno egg, log, crystal with no explore
    for consumable in ["Bobberfruit", "Medicinal Root", "Yellow Flower"]:
        set_rule(world.get_location(consumable), lambda state: (state.has_any(explore_jobs, world.player)),)

    # This line creates fill errors. Is it too restrictive?
    set_rule(world.get_location("Strange Device"), lambda state: (state.has("Explore the Ridge", world.player)),)


def set_job_rules(world: ExocolonistWorld) -> None:
    # Jobs are listed if they have that as the primary skill
    # OR the secondary is at least +2
    skills_to_job = {
        "empathy": ("Babysitting", "Barista", "Tending Animals", "Nursing Assistant", "Cooking"),
        "persuasion": ("Administration", "Leader", "Study Humanities", "Depot Clerk"),
        "creativity": ("Play the Photophonor", "Cooking", "Study Humanities", "Barista", "Robot Repair"),
        "bravery": ("Sportsball", "Sneak Out", "Explore Nearby", "Guard Duty"), # Not adding Explore Glow as that can only happen once a year
        
        "reasoning": ("Tutoring", "Study Engineering"),
        "organizing": ("Deliver Supplies", "Depot Clerk", "Xenobotany", "Administration"), # Not adding Rebuild as that is hard coded to year 15 only
        "engineering": ("Robot Repair", "Study Engineering", "Construction", "Survey the Ridge"),
        "biology": ("Nursing Assistant", "Xenobotany", "Study Life Sciences", "Farming", "Forage in the Valley"),

        "toughness": ("Shovelling Dirt", "Farming"), # Not adding Rebuild as that is hard coded to year 15 only
        "perception": ("Survey the Plains", "Forage in the Valley", "Survey the Ridge"),
        "combat": ("Defense Training", "Guard Duty", "Hunt in the Swamps"),
        "animals": ("Hunt in the Swamps", "Tending Animals"),
    }
    
    # Explore jobs require you to pass a check that requires 20 bravery or 20 toughness
    explore_jobs = ("Sneak Out", "Explore Nearby", "Survey the Plains", "Forage in the Valley", "Survey the Ridge", "Explore Glow")
    for job in explore_jobs:
        set_rule(
            world.get_location(job),
            lambda state: (state.has_any(skills_to_job["bravery"], world.player) or state.has_any(skills_to_job["toughness"], world.player)),
        )

    # Photophonor may require gear?

    # Study Engineering needs you to work one job there to unlock it
    engineering_jobs = ("Nursing Assistant", "Study Life Sciences", "Study Humanities", "Robot Repair", "Study Engineering", "Tutoring")
    set_rule(
        world.get_location("Study Engineering"),
        lambda state: (state.has_any(engineering_jobs, world.player)),
    )

    # Xenobotany requires 34 biology
    set_rule(
        world.get_location("Xenobotany"),
        lambda state: (state.has_any(skills_to_job["biology"], world.player)),
    )






def set_character_location_rules(world: ExocolonistWorld) -> None:
    chara_jobs = {
        "anemone": ("Sportsball", "Lookout Duty", "Defense Training"),
        "cal": ("Shovelling Dirt", "Farming", "Xenobotany", "Tending Animals"),
        "dys": ("Sneak Out", "Explore Nearby", "Survey the Plains", "Survey the Ridge", "Relax on the Walls"),
        "marz": ("Depot Clerk", "Administration", "Leader", "Deliver Supplies"),
        "nomi": ("Play the Photophonor", "Tutoring", "Robot Repair", "Study Engineering"),
        "rex": ("Deliver Supplies", "Construction", "Relax in the Lounge"),
        "tammy": ("Babysitting", "Cooking", "Relax in the Lounge"),
        "tang": ("Barista", "Study Life Sciences", "Study Humanities", "Nursing Assistant", "Study Engineering"),
        "vace": ("Guard Duty", "Hunt in the Swamps", "Defense Training"),
    }


    for chara in ["anemone", "cal", "dys", "marz", "tammy", "tang"]:
        set_rule(world.get_location(f"{chara.title()} 20"), lambda state: (state.has_any(chara_jobs[chara], world.player)),)
        for i in range(1,5):
            set_rule(
                world.get_location(f"{chara.title()} {(i+1)*20}"),
                lambda state: (state.has_any(chara_jobs[chara], world.player) and state.has("Progressive Year", world.player, i)),
            )
        set_rule(
            world.get_location(f"{chara.title()} Date"), 
            lambda state: (
                state.has_any(chara_jobs[chara], world.player) and state.has("Progressive Year", world.player, 5)
                ),
            )

    # These characters only appear year 5
    for chara in ["nomi", "rex", "vace"]:
        for i in range(0,5):
            set_rule(
                world.get_location(f"{chara.title()} {(i+1)*20}"),
                lambda state: (state.has_any(chara_jobs[chara], world.player) and state.has("Progressive Year", world.player, i+5)),
            )
        set_rule(
            world.get_location(f"{chara.title()} Date"), 
            lambda state: (
                state.has_any(chara_jobs[chara], world.player) and state.has("Progressive Year", world.player, 9)
                ),
            )



def set_victory_condition(world: ExocolonistWorld) -> None:
    world.multiworld.completion_condition[world.player] = lambda state: state.has("Ending", world.player)

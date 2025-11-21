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
  set_rule(age14_to_age15, lambda state: state.has("Progressive Year", world.player, 5) and (state.has("Mourn", world.player) or state.has("Rebuild", world.player)))
  set_rule(age15_to_age16, lambda state: state.has("Progressive Year", world.player, 6))
  set_rule(age16_to_age17, lambda state: state.has("Progressive Year", world.player, 7))
  set_rule(age17_to_age18, lambda state: state.has("Progressive Year", world.player, 8))
  set_rule(age18_to_age19, lambda state: state.has("Progressive Year", world.player, 9))
  set_rule(age19_to_age20, lambda state: state.has("Progressive Year", world.player, 10))

def set_all_location_rules(world: ExocolonistWorld) -> None:
  set_consumable_rules(world)
  set_job_rules(world)
  set_special_event_rules(world)
  if world.options.friendsanity:
    set_character_location_rules(world)
  if world.options.datesanity:
    set_dates_rules(world)
  if world.options.perksanity:
    set_skills_rules(world)


def set_consumable_rules(world: ExocolonistWorld) -> None:
  # Explore the Ridge and Explore Glow are Special
  explore_jobs = ("Sneak Out", "Explore Nearby", "Survey the Plains", "Forage in the Valley")
  
  # First year is xeno egg, log, crystal with no explore
  for consumable in ["Pick up Bobberfruit", "Pick up Medicinal Roots", "Pick up Yellow Flower"]:
    set_rule(
      world.get_location(consumable), 
      lambda state, explore_jobs=explore_jobs, bravery=world.skills_to_job["bravery"], toughness=world.skills_to_job["toughness"]: (
        state.has("Progressive Year", world.player, 1) and 
        state.has_any(explore_jobs, world.player) and 
        (
          state.has_any(bravery, world.player) or 
          state.has_any(toughness, world.player)
        )
      ),
    )

  set_rule(
    world.get_location("Pick up Strange Device"), 
    lambda state, bravery=world.skills_to_job["bravery"], toughness=world.skills_to_job["toughness"]: (
      state.has("Survey the Ridge", world.player) and 
      (
        state.has_any(bravery, world.player) or 
        state.has_any(toughness, world.player)
      )
    ),
  )


def set_job_rules(world: ExocolonistWorld) -> None:    
  # Explore jobs require you to pass a check that requires 20 bravery or 20 toughness
  for job in world.building_jobs["expeditions"]:
    # Explore Glow requires the ridge and 30 bravery
    if job == "Explore Glow":
      set_rule(
        world.get_location("Unlock Explore Glow"),
        lambda state, bravery=world.skills_to_job["bravery"]: (
          state.has("Survey the Ridge", world.player) and
          state.has_any(bravery, world.player)
        )
      )
    else:
      set_rule(
        world.get_location(f"Unlock {job}"),
        lambda state, bravery=world.skills_to_job["bravery"], toughness=world.skills_to_job["toughness"]: (
          state.has_any(bravery, world.player) or 
          state.has_any(toughness, world.player)
        ),
      )
  ###
  # Engineering
  # Study Engineering needs you to work one basic job there to unlock it
  basic_engineering_jobs = ("Study Life Sciences", "Study Humanities")
  set_rule(
    world.get_location("Unlock Study Engineering"),
    lambda state, basic_engineering_jobs=basic_engineering_jobs: (
      state.has_any(basic_engineering_jobs, world.player)
    ),
  )
  ###
  ###
  # Command
  # Requires 30 friendship with Marz
  set_rule(
    world.get_location("Unlock Depot Clerk"),
    lambda state, chara_jobs=world.chara_jobs["Marz"]: (
      state.has_any(chara_jobs, world.player)
    ),
  )
  # Construction requires 10 friendship with Rex
  set_rule(
    world.get_location("Unlock Construction"),
    lambda state, chara_jobs=world.chara_jobs["Rex"]: {
      state.has_any(chara_jobs, world.player)
    }
  )
  ###
  ###
  # Garrison
  # Guard Duty requires 10 bravery and 30 combat
  set_rule(
    world.get_location("Unlock Guard Duty"),
    lambda state, building_jobs=world.building_jobs, skills_to_job=world.skills_to_job: (
      state.has_any(building_jobs["garrison"], world.player) and
      state.has_any(skills_to_job["bravery"], world.player) and
      state.has_any(skills_to_job["combat"], world.player)
    )
  )
  ###
  ###
  # Geoponics
  # Tending Animals needs an explore job
  set_rule(
    world.get_location("Unlock Tending Animals"),
    lambda state, building_jobs=world.building_jobs: {
      state.has_any(building_jobs["geoponics"], world.player) and
      state.has_any(("Hunt in the Swamps", "Forage in the Valley", "Survey the plains"), world.player)
    }
  )
  if not world.options.perksanity:
    # Xenobotany requires 34 biology
    set_rule(
      world.get_location("Unlock Xenobotany"),
      lambda state, skills_to_job=world.skills_to_job: (
        state.has_any(skills_to_job["biology"], world.player)
      ),
    )
  # Relax in the Park is unlocked when you do geoponics jobs
  set_rule(
    world.get_location("Unlock Relax in the Park"),
    lambda state, building_jobs=world.building_jobs: (
      state.has_any(building_jobs["geoponics"], world.player)
    ),
  )
  job_reqs = [
    {   
      "name": "Tutoring",
      "building": "engineering",
      "skill": "reasoning",
      "threshold": 30
    },
    {
      "name": "Robot Repair",
      "building": "engineering",
      "skill": "engineering",
      "threshold": 50
    },
    {
      "name": "Nursing Assistant",
      "building": "engineering",
      "skill": "biology",
      "threshold": 50
    },
    {
      "name": "Construction",
      "building": "command",
      "skill": "toughness",
      "threshold": 30
    },
    {
      "name": "Lookout Duty",
      "building": "garrison",
      "skill": "perception",
      "threshold": 20
    },
    {
      "name": "Farming",
      "building": "geoponics",
      "skill": "biology",
      "threshold": 20
    },
    {
      "name": "Cooking",
      "building": "quarters",
      "skill": "organizing",
      "threshold": 20
    },
    {
      "name": "Barista",
      "building": "quarters",
      "skill": "empathy",
      "threshold": 67
    }
  ]
  for job in job_reqs:
    set_rule(
      world.get_location(f"Unlock {job['name']}"),
      lambda state, building_jobs=world.building_jobs[job["building"]], skill_jobs=world.skills_to_job[job["skill"]]: (
        state.has_any(building_jobs, world.player) and
        state.has_any(skill_jobs, world.player)
      ),
    )
  
  if world.options.perksanity:
    # If perksanity is enabled, Barista requires 2 empathy perks
    set_rule(
      world.get_location("Unlock Barista"),
      lambda state: {
        state.has("Progressive Empathy Perk", world.player, 2)
      }
    )
    # Robot Repair requires 1 engineering perk
    set_rule(
      world.get_location("Unlock Robot Repair"),
      lambda state: {
        state.has("Progressive Engineering Perk", world.player, 1)
      }
    )
    # Nursing Assistant requires 2 biology perks
    set_rule(
      world.get_location("Unlock Nursing Assistant"),
      lambda state: {
        state.has("Progressive Biology Perk", world.player, 2)
      }
    )
    # Xenobotany requires 1 biology perk
    set_rule(
      world.get_location("Unlock Xenobotany"),
      lambda state: (
        state.has("Progressive Biology Perk", world.player, 1)
      )
    )
    


def set_special_event_rules(world: ExocolonistWorld) -> None:
  # Tonin
  # Can be saved by sneaking out and finding his boss event with bravery 15 check
  # If you win it, you save him. If you lose you can reload or begin again
  set_rule(
    world.get_location("Save Tonin"),
    lambda state, bravery=world.skills_to_job["bravery"], toughness=world.skills_to_job["toughness"]: (
      state.has("Sneak Out", world.player) and 
      (
        state.has_any(bravery, world.player) or 
        state.has_any(toughness, world.player)
      )
    ),
  )
  # Hal
  # Can only be saved year 11 and seeing him die restarting, and poisoning his drink or
  # By passing combat check while sneaking out age 11
  set_rule(
    world.get_location("Save Hal"),
    lambda state, combat=world.skills_to_job["combat"]: (
      state.has_any(combat, world.player) and
      state.has("Sneak Out", world.player)
    ),
  )
  # Eudicot
  # Can save her by doing lookout duty during glow 14
  # Or passing 25 bravery check and combat battle
  set_rule(
    world.get_location("Save Eudicot"),
    lambda state, combat=world.skills_to_job["combat"], bravery=world.skills_to_job["bravery"]: (
      state.has_any(combat, world.player) and 
      state.has_any(bravery, world.player)
    ),
  )
  # Mom and Dad
  # Dad requires Forage in the Valley
  # Mom requires a high food count, best place is Forage in the Valley
  set_rule(
    world.get_location("Save Mom"),
    lambda state: (
      state.has("Forage in the Valley", world.player)
    ),
  )
  set_rule(
    world.get_location("Save Dad"),
    lambda state: (
      state.has("Forage in the Valley", world.player)
    ),
  )
  # Vriki
  # Logic will assume you're getting it via the glow event
  # Requires bravery 15 and perception 10 in year 12
  set_rule(
    world.get_location("Adopt Vriki"),
    lambda state, bravery=world.skills_to_job["bravery"], perception=world.skills_to_job["perception"]: (
      state.has_any(perception, world.player) and 
      state.has_any(bravery, world.player)
    ),
  )
  # Hopeye
  set_rule(
    world.get_location("Adopt Hopeye"),
    lambda state, bravery=world.skills_to_job["bravery"], toughness=world.skills_to_job["toughness"], animals=world.skills_to_job["animals"]: (
      (
        state.has("Sneak Out", world.player) or 
        state.has("Explore Nearby", world.player)
      ) and (
        state.has_any(bravery, world.player) or 
        state.has_any(toughness, world.player)
      ) and 
      state.has_any(animals, world.player) 
    ),
  )
  # Robot
  set_rule(
    world.get_location("Adopt Robot"),
    lambda state: (
      state.has("Robot Repair", world.player)
    ),
  )
  # Unisaur
  # Need to capture one while hunting (animals 40 check), then tend animals to tame it
  set_rule(
    world.get_location("Adopt Unisaur"),
    lambda state, bravery=world.skills_to_job["bravery"], toughness=world.skills_to_job["toughness"], animals=world.skills_to_job["animals"]: (
      (
        state.has_any(bravery, world.player) or 
        state.has_any(toughness, world.player)
      ) and
      state.has_any(animals, world.player) and
      state.has("Hunt in the Swamps", world.player) and
      state.has("Tending Animals", world.player)
    ),
  )
  # Governor events need at least one job in every building
  for event in ["Marz Governor", "Become Governor", "Unlock Leader"]:
    set_rule(
      world.get_location(event),
      lambda state, building_jobs=world.building_jobs: (
        state.has_any(building_jobs["garrison"], world.player) and
        state.has_any(building_jobs["engineering"], world.player) and
        state.has_any(building_jobs["quarters"], world.player) and
        state.has_any(building_jobs["command"], world.player) and
        state.has_any(building_jobs["geoponics"], world.player) and
        state.has_any(building_jobs["expeditions"], world.player)
      )
    )
  # Requires 100 persuasion, which you can only get with the third persuasion perk
  if world.options.perksanity:
    for event in ["Become Governor", "Unlock Leader"]:
      set_rule(
        world.get_location(event),
        lambda state: (
          state.has("Progressive Persuasion Perk", world.player, 3)
        )
      )


def set_character_location_rules(world: ExocolonistWorld) -> None:
  for chara in ["Anemone", "Cal", "Marz", "Tammy", "Tang"]:
    for i in range(20,110,10):
      set_rule(
        world.get_location(f"{chara} {i}"),
        lambda state, jobs=world.chara_jobs[chara]: (
          state.has_any(jobs, world.player)
        ),
      )
  # Dys has an extra requirement, we need bravery or toughness 20 for his jobs
  for i in range(20,110,10):
    set_rule(
      world.get_location(f"Dys {i}"),
      lambda state, jobs=world.chara_jobs["Dys"]: (
        state.has_any(jobs, world.player) and
        (
          state.has_any(world.skills_to_job["bravery"], world.player) or 
          state.has_any(world.skills_to_job["toughness"], world.player)
        )
      ),
    )
  # Sym is found in exploration jobs and only gains friendship in increments of 20
  for i in range(20,120,20):
    set_rule(
      world.get_location(f"Sym {i}"),
      lambda state, jobs=world.chara_jobs["Sym"]: (
        state.has_any(jobs, world.player) and
        (
          state.has_any(world.skills_to_job["bravery"], world.player) or 
          state.has_any(world.skills_to_job["toughness"], world.player)
        )
      ),
    )
  # These characters only appear year 5 and might have special rules
  for chara in ["Nomi", "Vace"]:
    for i in range(10,110,10):
      set_rule(
        world.get_location(f"{chara} {i}"),
        lambda state, jobs=world.chara_jobs[chara]: (
          state.has_any(jobs, world.player)
        ),
      )
  
  # I could not get to Rex 60 in logic with only Deliver Supplies, might need Construction for extra points
  for i in range(10,110,10):
    set_rule(
      world.get_location(f"Rex {i}"),
      lambda state: (
        state.has("Construction", world.player)
      ),
    )



def set_dates_rules(world: ExocolonistWorld) -> None:
  for chara in ["Anemone", "Cal", "Marz", "Tammy", "Tang", "Nomi", "Rex", "Vace"]:
    set_rule(
      world.get_location(f"Date {chara}"), 
      lambda state, jobs=world.chara_jobs[chara]: (
        state.has_any(jobs, world.player)
      ),
    )
  # Tammy requires high friendship with Cal
  set_rule(
    world.get_location(f"Date Tammy"), 
    lambda state, chara_jobs=world.chara_jobs: (
      state.has_any(chara_jobs["Cal"], world.player)
    ),
  )
  # Dys has an extra requirement, we need bravery or toughness 20 for most of his jobs
  # Sym is found in exploration jobs which also require bravery or toughness, so combining the two
  for chara in ["Dys", "Sym"]:
    set_rule(
      world.get_location(f"Date {chara}"), 
      lambda state, jobs=world.chara_jobs[chara], bravery=world.skills_to_job["bravery"], toughness=world.skills_to_job["toughness"]: (
        state.has_any(jobs, world.player) and
        (
          state.has_any(bravery, world.player) or 
          state.has_any(toughness, world.player)
        )
      ),
    )


def set_skills_rules(world):
  for skill in ["Empathy", "Creativity", "Persuasion", "Bravery", "Reasoning", "Engineering", "Organization", "Biology", "Toughness", "Combat"]:
    set_rule(
      world.get_location(f"{skill} Perk 1"),
      lambda state, skill=skill: (
        state.has_any(world.safe_skills_to_job[skill.lower()], world.player)
      )
    )
    for i in range(2,4):
      set_rule(
        world.get_location(f"{skill} Perk {i}"),
        lambda state, skill=skill, i=i: (
          state.has_any(world.safe_skills_to_job[skill.lower()], world.player) and
          state.has(f"Progressive {skill} Perk", world.player, i-1)
        )
      )
    
    # Perception and Animals only have one job associated with them
    for skill in ["Perception", "Animals"]:
      set_rule(
        world.get_location(f"{skill} Perk 1"),
        lambda state, skill=skill: (
          state.has(world.safe_skills_to_job[skill.lower()], world.player)
        )
      )
      for i in range(2,4):
        set_rule(
          world.get_location(f"{skill} Perk {i}"),
          lambda state, skill=skill, i=i: (
            state.has(world.safe_skills_to_job[skill.lower()], world.player) and
            state.has(f"Progressive {skill} Perk", world.player, i-1)
          )
        )




def set_victory_condition(world: ExocolonistWorld) -> None:
  world.multiworld.completion_condition[world.player] = lambda state: state.has("Ending", world.player)

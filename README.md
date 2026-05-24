# TODO
## Sanity
- every x seconds roll a dice to deduct points from sanity

## Door
- open door to hand over order

## Invenoty (maybe)
- HUD inventory display (1 slot)

## Dialogue
- Player.cs: handle_pick_up(), handle opening the door so the dialogue triggers according to the encounter type
	- normal/bizarre orders
	- human/monster portait

## Encounters
- roll die for x seconds to wait for (time_wait_encounter)
- roll die DEPENDENT on sanity value
- time_wait_encounter ends, trigger encounter according to 4die value: 
    - 0: human u think is human
		- evil_sfx_trigger = false
		- monster_portrait = false
		- bizarre_order = false
    - 1: human u think is monster         <--- increases the LESS sanity player has
		- evil_sfx_trigger = rand(0, 1) > sanity
		- monster_portrait = rand(0, 1) > sanity
		- bizarre_order = rand(0, 1) > sanity
    - 2: monster
		- evil_sfx_trigger = true
		- monster_portrait = true
		- bizarre_order = true
    - 3: monster disguised as human
		- evil_sfx_trigger = rand(0, 1)
		- monster_portrait = rand(0, 1)
		- bizarre_order = rand(0, 1)

- MAYBE add hallucinations as an indicator but idk if we'll have time :')

# EncounterSystem
- handle interractions with door
	- run arrivalAtDoorTimer
	- encounter knocks the door
	- player interracts with door (e)
	- dialogue 
	- spawn order of pills
	- player interracts with door again (q)
	- play 'are you sure' dialogue
		- if has pills
			- if mimic or monster
				- dead
			- else
				- remove encounter
				- proceed to next encounter
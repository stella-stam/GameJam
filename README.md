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
- roll 4die DEPENDENT on sanity value
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

probability of option 1 (hallucination), P(hallucination) = (1 - sanity)*0.8

probability of option 0/2/3, P(others) = (1 - P(hallucination)) / 3


rand = rng.Next()

if rand < P(hallucination) {
	encouter = 1
}

else if rand < P(hallucination) + P(others) {
	encounter = 0
}

else if rand < P(hallucination) + 2*P(others)  {
	encounter = 2
}

else  {
	encounter = 3
}
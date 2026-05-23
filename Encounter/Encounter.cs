using Godot;
using System;

public partial class Encounter : Node2D
{
	enum EncounterType
	{
		Human,			 // 0: Human you think is human
		Hallucination,   // 1: Human you think is monster
		Monster,		 // 2: Monster 
		Mimic 			 // 3: Monster disguised as human
	}

	Random rng = new Random();

	EncounterType encounter;

	Timer encounterTimer;

	Timer encounterSoundTimer;

	double playerSanity;

	public bool isEncounterAtDoor = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		encounterTimer = GetNode<EncounterTimer>("../EncounterTimer");

		// Wait for 20-60 seconds for first encounter
		encounterTimer.WaitTime = rng.Next(2, 6);

		// Wait a bit less to start encounter SFX (if necessary)
		if (encounter != EncounterType.Human)
		{
			encounterSoundTimer.WaitTime =  encounterTimer.WaitTime * rng.Next();
		}

		selectEncounter();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		playerSanity = GetNode<Player>("../Player").sanity;
	}

	private void OnEncounterTimerTimeout()
	{	
		isEncounterAtDoor = true;
	}

	private void OnEncounterSoundTimerTimeout()
	{
		
	}

	private void selectEncounter()
	{
		double probabilityOfHallucination = (1 - playerSanity) * 0.8;

		double probabilityOfOthers = 1 - probabilityOfHallucination;

		double randomNumber = rng.Next();

		if (randomNumber < probabilityOfHallucination) {
			encounter = (EncounterType)1;
		}

		else if (randomNumber < probabilityOfHallucination + probabilityOfOthers) {
			encounter = (EncounterType)0;
		}

		else if (randomNumber < probabilityOfHallucination + 2 * probabilityOfOthers)  {
			encounter = (EncounterType)2;
		}

		else  {
			encounter = (EncounterType)3;
		}
	}
}

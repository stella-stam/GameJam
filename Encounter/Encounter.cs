using Godot;
using System;

public partial class Encounter : Node2D
{
	public enum EncounterType
	{
		Human,           // 0: Human you think is human
		Hallucination,   // 1: Human you think is monster
		Monster,         // 2: Monster 
		Mimic            // 3: Monster disguised as human
	}

	Random rng = new Random();

	EncounterType _type;
	public EncounterType type => _type;

	double playerSanity;

	public bool isEncounterAtDoor = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		playerSanity = GameManager.Instance.player.sanity;
	}

	private void OnEncounterTimerTimeout()
	{
		isEncounterAtDoor = true;
	}

	private void OnEncounterSoundTimerTimeout()
	{

	}

	public void SelectEncounter()
	{
		double probabilityOfHallucination = (1 - playerSanity) * 0.8;

		double probabilityOfOthers = 1 - probabilityOfHallucination;

		double randomNumber = rng.Next();

		if (randomNumber < probabilityOfHallucination)
		{
			_type = (EncounterType)1;
		}

		else if (randomNumber < probabilityOfHallucination + probabilityOfOthers)
		{
			_type = (EncounterType)0;
		}

		else if (randomNumber < probabilityOfHallucination + 2 * probabilityOfOthers)
		{
			_type = (EncounterType)2;
		}

		else
		{
			_type = (EncounterType)3;
		}
	}
}

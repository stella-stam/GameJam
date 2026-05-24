using Godot;
using System;

public partial class EncounterSystem : Node2D
{
	Random rng = new Random();

	Encounter encounter;

	Character character;

	[Export]
	Timer doorArrivalTimer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		doorArrivalTimer.WaitTime = 5;

		doorArrivalTimer.Start();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	void OnDoorArrivalTimerTimeout()
	{
		doorArrivalTimer.SetPaused(true);
		GD.Print("knock knock");
	}
}

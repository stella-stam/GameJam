using Godot;
using System;

public partial class CharacterSys : Node2D
{
	[Export]
	Timer doorArrivalTimer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
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

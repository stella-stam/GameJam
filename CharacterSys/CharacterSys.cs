using Godot;
using Godot.Collections;
using System;

public partial class CharacterSys : Node2D
{
	[Export]
	public Array<Character> characters;

	Character activeCharacter;
	public Character ActiveCharacter => activeCharacter;

	int charasProcessed = 0;

	[Export]
	Timer doorArrivalTimer;
	public override void _Ready()
	{
		doorArrivalTimer.Start();
	}

	void OnDoorArrivalTimerTimeout()
	{
		doorArrivalTimer.SetPaused(true);
		activeCharacter = characters[charasProcessed];

		if (charasProcessed == 4)
		{
			GameManager.Instance.win();
		}
	}

	public void OnReqCompleted()
	{
		activeCharacter = characters[1];
	}
}

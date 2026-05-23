using Godot;
using System;

public partial class Player : CharacterBody2D
{
	private enum encounterType
	{
		Human,			 // 0: Human you think is human
		Hallucination,   // 1: Human you think is monster
		Monster,		 // 2: Monster 
		Mimic 			 // 3: Monster disguised as human
	}

	[Export]
	public int Speed { get; set;} = 400;

	public Vector2 ScreenSize;

	private Item pickupable_item;

	private Item inventory;

	Random rng;

	encounterType encounter;

	Timer encounterTimer;

	bool encounterAtDoor = false;

	bool nearDoor = false;

	// Takes value in [0, 1], 0 is insanity, 1 is full sanity
	private double sanity = 1.0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;

		sanity = 1.0;

		rng = new Random();

		encounterTimer = GetNode<EncounterTimer>("../EncounterTimer");

		// Wait for 20-60 seconds for first encounter
		encounterTimer.WaitTime = rng.Next(2, 6);

		encounter = (encounterType)rng.Next(0, 4);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		handle_input_movement(delta);

		handle_pick_up();

		handle_use();
	}

	private void handle_input_movement(double delta)
	{
		var velocity = Vector2.Zero;

		if (Input.IsActionPressed("right"))
		{
			velocity.X += 1;
		}

		if (Input.IsActionPressed("left"))
		{
			velocity.X -= 1;
		}

		if (Input.IsActionPressed("down"))
		{
			velocity.Y += 1;
		}

		if (Input.IsActionPressed("up"))
		{
			velocity.Y -= 1;
		}

		velocity = velocity.Normalized();

		Velocity = velocity * Speed;

		MoveAndSlide();
	}

	private void handle_pick_up()
	{
		if (Input.IsActionPressed("pick_up")) {
			if (pickupable_item != null) 
			{
				inventory = pickupable_item;
				pickupable_item = null;
				inventory.QueueFree();

				GD.Print("Inventory contains: ", inventory.ID);
			}

			else if (nearDoor)
			{
				GD.Print("opening door");
				// TODO: trigger dialogue according to encounter type
			}
		}
	}

	private void handle_use()
	{
		if (Input.IsActionPressed("use") && inventory != null) {
			GD.Print("use triggered");
			if (inventory.ID == "antipsychotics") {
				sanity = Math.Clamp(sanity + 0.4, 0.0, 1.0);

				GD.Print("Sanity: ", sanity);
			}

			inventory = null;
		}
	}

	private void OnArea2dAreaEntered(Area2D area) 
	{
		pickupable_item = area.GetParent<Item>();
	}

	private void OnSanityTimerTimeout() 
	{
		sanity = Math.Clamp(sanity - rng.NextSingle() * 0.1, 0.0, 1.0);

		GD.Print("sanity: ", sanity);
	}

	private void OnEncounterTimerTimeout()
	{	
		encounterAtDoor = true;
	}

	private void NearDoor(Area2D area)
	{
		nearDoor = true;
	}
}

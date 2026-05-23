using Godot;
using System;

public partial class Player : CharacterBody2D
{
	Random rng = new Random();

	Item pickupableItem;

	Item inventory;

	bool isNearDoor = false;

	int speed { get; set;} = 400;

	Vector2 screenSize;

	// Takes value in [0, 1], 0 is insanity, 1 is full sanity
	public double sanity = 1.0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		screenSize = GetViewportRect().Size;
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

		Velocity = velocity * speed;

		MoveAndSlide();
	}

	private void handle_pick_up()
	{
		if (Input.IsActionPressed("pick_up")) {
			if (pickupableItem != null) 
			{
				inventory = pickupableItem;
				pickupableItem = null;
				inventory.QueueFree();

				GD.Print("Inventory contains: ", inventory.ID);
			}

			else if (isNearDoor)
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

	private void OnSanityTimerTimeout() 
	{
		sanity = Math.Clamp(sanity - rng.NextSingle() * 0.1, 0.0, 1.0);

		GD.Print("sanity: ", sanity);
	}

	private void OnAreaEntered(Area2D area)
	{		
	    var parent = area.GetParent();

	    if (parent.IsInGroup("items"))
	    {
	        pickupableItem = (Item)parent;
	        GD.Print("Near item ID: ", pickupableItem.ID); 
	    }
	    else if (parent.IsInGroup("door"))
	    {
	        isNearDoor = true;
	        GD.Print("Is near door");
	    }
	}
}

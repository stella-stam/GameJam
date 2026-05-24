using Godot;
using System;
using System.ComponentModel;

public partial class Player : CharacterBody2D
{
	Random rng = new Random();

	Item pickupableItem;

	public Item.ItemType heldItem;

	bool isNearDoor = false;

	int speed { get; set; } = 400;

	Vector2 screenSize;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		screenSize = GetViewportRect().Size;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		handle_use();
	}

	public override void _PhysicsProcess(double delta)
	{


		handle_input_movement(delta);

		handle_pick_up();
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
		if (Input.IsActionPressed("pick_up"))
		{
			if (pickupableItem != null)
			{
				GD.Print("Picked up: " + pickupableItem.type);
				heldItem = pickupableItem.type;
				pickupableItem.QueueFree();
				pickupableItem = null;
			}
		}
	}

	private void handle_use()
	{
		if (Input.IsActionJustPressed("use") && isNearDoor)
		{
			GameManager.Instance.gm.OnDoorInteracted();
		}
	}

	private void OnAreaEntered(Area2D area)
	{
		var parent = area.GetParent();

		GD.Print("Near interactable: ", parent.Name);

		if (parent is Item item)
		{
			pickupableItem = item;
		}

		else if (parent is Door)
		{
			isNearDoor = true;
		}
	}

	private void OnAreaExited(Area2D area)
	{
		var parent = area.GetParent();

		if (parent is Item item)
		{
			pickupableItem = null;
		}

		else if (parent is Door door)
		{
			isNearDoor = false;
		}
	}
}

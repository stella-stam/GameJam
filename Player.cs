using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export]
	public int Speed { get; set;} = 400;

	public Vector2 ScreenSize;

	private Item pickupable_item;

	private Item inventory;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public override void _PhysicsProcess(double delta)
	{
		handle_input_movement(delta);

		if (Input.IsActionPressed("pick_up"))
		{
			handle_pick_up();
		}
	}

	private void OnArea2dAreaEntered(Area2D area) 
	{
		pickupable_item = area.GetParent<Item>();
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
		if (pickupable_item != null)
		{
			inventory = pickupable_item;
			pickupable_item = null;
			inventory.QueueFree();

			GD.Print("Inventory contains: ", inventory.Title);
		} 
	}
}

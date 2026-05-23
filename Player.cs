using Godot;
using System;

public partial class Player : Node2D
{
	[Export]
	public int Speed { get; set;} = 400;

	public Vector2 ScreenSize;

	private Borders borders;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;

		borders = GetNode<Borders>("../Borders");

		GD.Print("Borders: ", borders.Position, borders.Size);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		handle_input_movement(delta);
	}

	private void handle_input_movement(double delta)
	{
		var velocity = Vector2.Zero; // The player's movement vector.

		if (Input.IsActionPressed("right"))
		{
			GD.Print("right pressed");
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
		
		Position += velocity * (float)delta * Speed;

		Position = new Vector2(
			x: Mathf.Clamp(Position.X, borders.Position.X, borders.Position.X + borders.Size.X),
			y: Mathf.Clamp(Position.Y, borders.Position.Y, borders.Position.Y + borders.Size.Y)
		);

	}
}

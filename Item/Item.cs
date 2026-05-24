using Godot;
using System;

[GlobalClass]
public partial class Item : Node2D, IInteractable
{
	public enum ItemType
	{
		NONE,
		KEYS,
		PILL_ASPIRIN,
		PILL_IBU_RED,
		PILL_SANITY

	}

	[Export]
	public ItemType type;
	public void Interact()
	{

	}

}

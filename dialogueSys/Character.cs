
using System.Collections;
using Godot;
using Godot.Collections;

[GlobalClass]
public partial class Character : Resource
{
    [Export]
    public int uid;
    [Export]
    public Texture2D sprite;
    [Export]
    public Godot.Vector2 spriteOffset;
    [Export]
    public Color dialogueTextColor;
    [Export]
    public int dialogueStartId;
    [Export]
    public int dialogueCompleteID;
    [Export]
    public bool isMonster = false;


    [Export]
    public Array<Item.ItemType> request = new();
}
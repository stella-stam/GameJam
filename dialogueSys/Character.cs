using System.Numerics;
using System.Runtime.ConstrainedExecution;
using Godot;

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
    public int dialogueEncounterId;
    [Export]
    public int dialogueCounterId;
}
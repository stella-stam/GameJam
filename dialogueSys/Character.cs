using System.Numerics;
using System.Runtime.ConstrainedExecution;
using Godot;

[GlobalClass]
public partial class Character : Resource
{
    public int uid;
    public Texture2D sprite;
    public Godot.Vector2 spriteOffset;
    public Color dialogueTextColor;

    public int dialogueEncounterId;
    public int dialogueCounterId;
}
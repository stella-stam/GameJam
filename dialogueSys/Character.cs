using System.Numerics;
using System.Runtime.ConstrainedExecution;
using Godot;

public partial class Character : Resource
{
    public int uid;
    public bool isEnemy = false;
    public Texture2D sprite;
    public Godot.Vector2 spriteOffset;
    public Color dialogueTextColor;
}
using Godot;

public partial class GameManager : Node
{
    public static GameManager Instance;
    [Export]
    public DialogueSystem dialogueSys;
    [Export]
    public Encounter encounterSys;
    [Export]
    public Player player;

    public override void _Ready()
    {
        Instance = this;
    }
}
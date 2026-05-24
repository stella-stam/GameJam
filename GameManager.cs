using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;

public partial class GameManager : Node
{
    public static GameManager Instance;
    [Export]
    public DialogueSystem dialogueSys;
    [Export]
    public CharacterSys characterSys;
    [Export]
    public Player player;
    [Export]
    public Gameplay gm;

    public override void _Ready()
    {
        Instance = this;
    }
}
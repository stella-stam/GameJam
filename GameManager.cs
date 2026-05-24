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

    [Export]
    public PackedScene EnemyScene;

    public void SpawnEnemyInRadius(Vector2 centerPosition, float radius, float speed)
    {
        if (EnemyScene == null) return;
        float randomAngle = (float)GD.RandRange(0.0f, Mathf.Tau);
        Vector2 offset = Vector2.FromAngle(randomAngle) * radius;
        Vector2 spawnPosition = centerPosition + offset;
        Node2D newEnemy = (Node2D)EnemyScene.Instantiate();
        newEnemy.GlobalPosition = spawnPosition;

        if (newEnemy is ChargingEnemy chargingEnemy)
        {
            chargingEnemy.SpeedScale = speed;
        }

        AddChild(newEnemy);
    }

    [Export]
    public Godot.Collections.Dictionary<Item.ItemType, PackedScene> items;

    [Export]
    public Array<Node> spawnPoints = new();

    // Tracks which nodes currently have an item sitting on them
    private HashSet<Node> _occupiedPoints = new();
    private Random _random = new Random();

    public void SpawnRandom(Item.ItemType type)
    {
        if (spawnPoints == null || spawnPoints.Count == 0)
        {
            GD.PrintErr("SpawnRandom: spawnPoints list is empty!");
            return;
        }

        List<Node> availablePoints = new List<Node>();
        foreach (Node point in spawnPoints)
        {
            if (!_occupiedPoints.Contains(point))
            {
                availablePoints.Add(point);
            }
        }

        if (availablePoints.Count == 0)
        {

            return;
        }

        int randomIndex = _random.Next(availablePoints.Count);
        Node pickedNode = availablePoints[randomIndex];

        if (pickedNode is Node2D spawnPoint2D)
        {
            // Mark this point as occupied before instantiating
            _occupiedPoints.Add(pickedNode);

            Vector2 p = spawnPoint2D.GlobalPosition;
            SpawnItem(type, p, pickedNode);
        }
        else
        {
            GD.PrintErr($"SpawnRandom: Element is not a Node2D/Marker2D!");
        }
    }

    public void SpawnItem(Item.ItemType type, Vector2 spawn, Node originPoint)
    {
        if (!items.ContainsKey(type)) return;

        PackedScene itemScene = items[type];
        if (itemScene == null)
        {
            GD.PrintErr($"SpawnItem: The PackedScene for '{type}' is unassigned!");
            return;
        }

        Node2D newItem = (Node2D)itemScene.Instantiate();
        newItem.GlobalPosition = spawn;
        AddChild(newItem);

        newItem.TreeExited += () => OnItemPickedUp(originPoint);
    }

    private void OnItemPickedUp(Node originPoint)
    {
        if (_occupiedPoints.Contains(originPoint))
        {
            _occupiedPoints.Remove(originPoint);
        }
    }

    public override void _Ready()
    {
        Instance = this;
    }

    public void win()
    {
        GD.Print("Processed Enough Characters to win!");
    }

    public void lose()
    {
        GD.Print("Processed Enough Characters to win!");
    }

}
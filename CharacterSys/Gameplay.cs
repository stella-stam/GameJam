using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Godot;

public partial class Gameplay : Node
{
    bool isDeliveringItems = false;
    Character ActiveCharacter => GameManager.Instance.characterSys.ActiveCharacter;
    bool hasAcceptedDoor = false;
    DialogueSystem sys => GameManager.Instance.dialogueSys;
    Player plr => GameManager.Instance.player;

    List<Item.ItemType> request = new();

    bool setReq = false;

    public override void _Ready()
    {

    }

    public override void _Process(double delta)
    {

    }

    public void OnDoorInteracted()
    {
        if (ActiveCharacter == null)
        {
            //nobody there
            return;
        }

        if (!hasAcceptedDoor && !setReq)
        {
            //prompt player to awnser door
            sys.StartDialogue(0);
            return;
        }


        if (setReq)
        {
            GD.Print("Held:" + plr.heldItem);
            //open door prompt
            sys.StartDialogue(25);
            GD.Print("Open door prompt");
            return;
        }


        //repeat req dialogue
        sys.StartDialogue(10);

        GD.Print("repeat request");
        SetRequest();
    }
    public void SetRequest()
    {
        if (!setReq)
        {
            request.AddRange(ActiveCharacter.request);
            setReq = true;
            GD.Print("Set req: " + request[0]);
        }
        //spawn items
    }
    public void OnDoorAwnser()
    {
        //play req dialogue
        hasAcceptedDoor = true;
        sys.StartDialogue(ActiveCharacter.dialogueStartId);
        GD.Print("Awnsered door");
        SetRequest();
    }

    public void OnDoorOpen()
    {
        if (ActiveCharacter.isMonster)
        {
            // die();
            GD.Print("Game over. Character is a monster");
        }
        else
        {
            if (plr.heldItem != Item.ItemType.NONE && request.Contains(plr.heldItem))
            {
                //right item
                var item = plr.heldItem;
                plr.heldItem = Item.ItemType.NONE;
                request.Remove(item);
            }

            if (request.Count == 0)
            {
                GD.Print("Completed req");
                sys.StartDialogue(ActiveCharacter.dialogueCompleteID);
                onReqCompleted();
            }
        }
    }

    public void onReqCompleted()
    {
        setReq = false;
        request.Clear();
        hasAcceptedDoor = false;

    }

    public void OnDoorIgnore()
    {
        //walk away after delay. delay not yet
        sys.StartDialogue(22);

        GD.Print("ignored door");

        if (ActiveCharacter.isMonster)
        {
            //play walk away sfx
        }
        else
        {

        }
    }
}
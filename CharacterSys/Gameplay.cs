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

    float timer;
    bool hasComplained = false;

    bool playIntro = true;



    public override void _Ready()
    {

    }


    public override void _Process(double delta)
    {
        if (ActiveCharacter != null)
        {
            if (playIntro)
            {
                sys.StartDialogue(4);
                playIntro = false;
            }

            // if (setReq && !GameManager.Instance.dialogueSys.IsInDialogue)
            //     timer += (float)delta;

            // if (timer > 8 && !hasComplained)
            // {
            //     //character is complaining
            //     sys.StartDialogue(ActiveCharacter.ignoringKnockID);
            //     hasComplained = true;
            // }

            if (timer > 19)
            {
                OnDoorIgnore(true);
                timer = 0;
            }
        }
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

            foreach (var item in request)
            {
                GameManager.Instance.SpawnRandom(item);
            }
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
        hasComplained = false;
        hasAcceptedDoor = false;
        GD.Print("Completed request");
        GameManager.Instance.characterSys.OnReqCompleted();
    }

    public void OnDoorIgnore(bool fin)
    {
        //walk away after delay. delay not yet set

        if (fin)
        {
            sys.StartDialogue(22);

            if (ActiveCharacter.isMonster)
            {
                //play walk away sfx
                //yay +1 points
                onReqCompleted();
            }
            else
            {
                // normal customer left
            }
        }
        else
        {
            sys.StartDialogue(2);
        }
    }
}
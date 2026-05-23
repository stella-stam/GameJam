using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public partial class DialogueSystem : Node
{
    public int currentNodeId = -1;

    //characters that the dialogue system can display
    public List<Character> characters = new();
    public Character MAIN_CHARA;
    public Character TEST_CHARA;

    public Dictionary<int, Dialogue> allGameDialogue = new();

    public bool isOnChoiceNode = false;

    [Export]
    CanvasLayer textboxUI;
    [Export]
    Label textbox;

    public override void _Ready()
    {
        textboxUI.Visible = false;
        DefineAllDialogue();
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("ui_accept"))
        {
            ProgressDialogue();
        }

        if (Input.IsKeyPressed(Key.O))
        {
            StartDialogue(0);
        }

        if (Input.IsKeyPressed(Key.P))
        {
            StartDialogue(10);
        }
    }


    public void StartDialogue(int startNodeID)
    {
        SetCurrentNode(startNodeID);
    }

    private void DefineAllDialogue()
    {
        //REGISTER ALL GAME DIALOGUE HERE. BECAUSE IM LAZY.
        Add(new Dialogue(0, 0, "This is a sample dialogue"));
        Add(new Dialogue(1, 0, "and so"));
        Add(new Dialogue(2, 0, "life continues as usual"));
        Add(new Dialogue(3, 0, "yknow"));
        Add(new Dialogue(4, 0, "whatever!").next(0));

        Add(new Dialogue(10, 0, "no hoes"));
        Add(new Dialogue(11, 0, "no bitches"));
        Add(new Dialogue(12, 0, "and probably no balls either"));
        Add(new Dialogue(13, 0, "NAH!").end());
    }

    private void ProgressDialogue()
    {
        if (currentNodeId < 0)
        {
            GD.Print("No dialogue active (ID -1)");
            return;
        }

        if (allGameDialogue.ContainsKey(currentNodeId))
        {
            var node = allGameDialogue[currentNodeId];

            if (node is ChoiseDialogue choise)
            {
                //cant progress until the player makes a choise.
                isOnChoiceNode = true;
                return;
            }
            else
            {
                //auto progress to the next node pointed to by node.nextid
                int nextNodeId = node.GetNextNodeId();
                SetCurrentNode(nextNodeId);

                isOnChoiceNode = false;
            }
        }
        else
        {
            GD.Print("currentNodeId illegal value of" + currentNodeId);
            return;
        }
    }

    private void SetCurrentNode(int newval)
    {
        currentNodeId = newval;
        if (currentNodeId < 0)
        {
            //hide dialogue scene
            GD.Print("Dissabled dialogue!");
            textboxUI.Visible = false;
        }
        else
        {
            //show dialogue scene
            GD.Print("progressed dialogue: id" + currentNodeId + ", \"" + allGameDialogue[currentNodeId].text + "\"");
            textbox.Text = allGameDialogue[currentNodeId].text;
            textboxUI.Visible = true;
        }
        GD.Print("set: " + newval);
    }

    private void Add(Dialogue node)
    {
        allGameDialogue[node.uid] = node;
    }
}

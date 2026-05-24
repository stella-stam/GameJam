using Godot;
using Godot.Collections;
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

    public System.Collections.Generic.Dictionary<int, Dialogue> allGameDialogue = new();

    [Export]
    Control textboxUI;
    [Export]
    Label textbox;
    [Export]
    Control OptionsUI;
    [Export]
    Button Opt1Button;
    [Export]
    Button Opt2Button;

    [Export]
    Control images;

    public override void _EnterTree()
    {
        textboxUI.Visible = false;
        OptionsUI.Visible = false;
        images.Visible = false;
        DefineAllDialogue();
        Opt1Button.Pressed += () => OnButtPressed(1);
        Opt2Button.Pressed += () => OnButtPressed(2);
    }


    private void OnButtPressed(int v)
    {
        GD.Print("Butt was pressed");
        var node = allGameDialogue[currentNodeId] as ChoiseDialogue;
        if (v == 1)
        {
            SetCurrentNode(node.option1.nextNodeId);
            node.option1.callback?.Invoke();
        }
        else
        {
            SetCurrentNode(node.option2.nextNodeId);
            node.option2.callback?.Invoke();
        }
    }


    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("ui_accept"))
        {
            ProgressDialogue();
        }
    }


    public void StartDialogue(int startNodeID)
    {
        SetCurrentNode(startNodeID);
    }

    private void DefineAllDialogue()
    {
        //REGISTER ALL GAME DIALOGUE HERE. BECAUSE IM LAZY.
        Add(new ChoiseDialogue(0, 0, "Reply?")
            .Option1(new ChoiseDialogue.Option("Yes", -1, () => { GameManager.Instance.gm.OnDoorAwnser(); }))
            .Option2(new ChoiseDialogue.Option("No", -1, () => { GameManager.Instance.gm.OnDoorIgnore(); }))
            .noimg()
        );
        Add(new Dialogue(2, 0, "What is taking you so long?").noimg());
        Add(new Dialogue(3, 0, "Come on Adrian").noimg().end());

        Add(new ChoiseDialogue(25, 0, "Open the door?")
            .Option1(new ChoiseDialogue.Option("Yes", -1, () => { GameManager.Instance.gm.OnDoorOpen(); }))
            .Option2(new ChoiseDialogue.Option("No", -1, () => { GameManager.Instance.gm.OnDoorIgnore(); }))
            .noimg()
        );

        Add(new Dialogue(10, 0, "Hey, finally."));
        Add(new Dialogue(11, 0, "I forgot my home keys during my last shift, go get em wont you?"));
        Add(new Dialogue(12, 0, "They are on the main desk, next to your meds. Your uncle is on the next shift, just so you know."));
        Add(new Dialogue(13, 0, "Well? What are you waiting for?").end());

        Add(new Dialogue(15, 0, "There, thanks."));
        Add(new Dialogue(16, 0, "Did you take your pills?"));
        Add(new Dialogue(17, 0, "Just make sure to keep yourself awake, there are plenty of people coming and going even tonight."));
        Add(new Dialogue(18, 0, "..ill get going. Dont get into a fight again, we just had the door repaired. You arent getting second chances"));
        //change portrait here to illusion one. sfx?
        Add(new Dialogue(19, 0, "ill in the head or not.").end());



        Add(new Dialogue(22, 0, "The stranger walks away.").noimg().end());

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
                return;
            }
            else
            {
                //auto progress to the next node pointed to by node.nextid
                int nextNodeId = node.GetNextNodeId();
                SetCurrentNode(nextNodeId);
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
            OptionsUI.Visible = false;
            images.Visible = false;
        }
        else
        {
            var node = allGameDialogue[currentNodeId];
            //show dialogue scene
            GD.Print("progressed dialogue: id" + currentNodeId + ", \"" + allGameDialogue[currentNodeId].text + "\"");
            textbox.Text = allGameDialogue[currentNodeId].text;
            textboxUI.Visible = true;
            images.Visible = !node.hideImages;

            if (node is ChoiseDialogue dial)
            {
                OptionsUI.Visible = true;
                Opt1Button.Text = dial.option1.optionText;
                Opt2Button.Text = dial.option2.optionText;
            }
            else
            {
                OptionsUI.Visible = false;
            }
        }
        GD.Print("set: " + newval);
    }

    private void Add(Dialogue node)
    {
        allGameDialogue[node.uid] = node;
    }



}

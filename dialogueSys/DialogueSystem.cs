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
    SlowTextLabel tb;
    [Export]
    Control OptionsUI;
    [Export]
    Button Opt1Button;
    [Export]
    Button Opt2Button;

    [Export]
    Control images;

    [Export]
    Sprite2D charImg;

    Texture2D overridePortrait;

    Array<Character> gameCharacters => GameManager.Instance.characterSys.characters;

    bool isInDialogue = false;
    public bool IsInDialogue => isInDialogue;

    float minWaitTime = 1;
    float cWaitTime = 1;

    public void SetOverridePortrait(Texture2D text)
    {
        overridePortrait = text;
    }

    public void ClearOverridePortrait()
    {
        overridePortrait = null;
    }

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
        if (cWaitTime > 0)
        {
            cWaitTime -= (float)delta;
        }
        if (Input.IsActionJustPressed("ui_accept") && cWaitTime <= 0)
        {
            ProgressDialogue();
            cWaitTime = minWaitTime;
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
            .Option2(new ChoiseDialogue.Option("Stay silent", -1, () => { GameManager.Instance.gm.OnDoorIgnore(false); }))
            .noimg()
        );
        Add(new Dialogue(4, 1, "Adrian?").noimg());
        Add(new Dialogue(5, 1, "*knock knock*").noimg().end());

        Add(new Dialogue(2, 1, "What is taking you so long?").noimg());
        Add(new Dialogue(3, 1, "Come on Adrian").noimg().end());

        Add(new ChoiseDialogue(25, 0, "Open the door?")
            .Option1(new ChoiseDialogue.Option("Yes", -1, () => { GameManager.Instance.gm.OnDoorOpen(); }))
            .Option2(new ChoiseDialogue.Option("No, Lock the door", -1, () => { GameManager.Instance.gm.OnDoorIgnore(true); }))
            .noimg()
        );

        Add(new Dialogue(10, 1, "Hey, finally."));
        Add(new Dialogue(11, 1, "I forgot my home keys during my last shift, go get em wont you?"));
        Add(new Dialogue(12, 1, "They are on the main desk, next to your meds. Your uncle is on the next shift, just so you know."));
        Add(new Dialogue(13, 1, "Well? What are you waiting for?").end());

        Add(new Dialogue(15, 1, "There, thanks."));
        Add(new Dialogue(16, 1, "Did you take your pills?"));
        Add(new Dialogue(17, 1, "Just make sure to keep yourself awake, there are plenty of people coming and going even tonight."));
        Add(new Dialogue(18, 1, "..ill get going. Dont get into a fight again, we just had the door repaired. You arent getting second chances")
        .callback(() =>
        {
            GD.Print("Changed portrait");
            GD.Print(gameCharacters[0].sprite_alt);
            SetOverridePortrait(gameCharacters[0].sprite_alt);
        }));
        //change portrait here to illusion one. sfx?
        Add(new Dialogue(19, 1, "ill in the head or not.").end());
        Add(new Dialogue(22, 0, "The stranger walks away.").noimg().end());

        Add(new Dialogue(30, 1, "Hello dearie..oh why do you look so down?").callback(() =>
        {
            SetOverridePortrait(gameCharacters[1].sprite);
        }));
        Add(new Dialogue(31, 1, "I need some pills, those, the red ones..that make you happy! That make you smile!").callback(() =>
        {
            SetOverridePortrait(gameCharacters[1].sprite);
        }));
        Add(new Dialogue(32, 1, "But you should smile. You should smile a lot more..").callback(() =>
        {
            SetOverridePortrait(gameCharacters[1].sprite);
        }));
        Add(new Dialogue(33, 1, "You should smile.").callback(() =>
        {
            SetOverridePortrait(gameCharacters[1].sprite_alt);
        }));
        Add(new Dialogue(34, 1, "You should smile.").callback(() =>
        {
            SetOverridePortrait(gameCharacters[1].sprite_alt);
        }));
        Add(new Dialogue(35, 1, "You should smile.").end().callback(() =>
        {
            SetOverridePortrait(gameCharacters[1].sprite_alt);
            GameManager.Instance.SpawnEnemyInRadius(new Vector2(314, 0), 200, 200);
            GameManager.Instance.SpawnEnemyInRadius(new Vector2(314, 0), 200, 100);
            GameManager.Instance.SpawnEnemyInRadius(new Vector2(314, 0), 200, 300);
            GameManager.Instance.SpawnEnemyInRadius(new Vector2(314, 0), 200, 200);
            GameManager.Instance.SpawnEnemyInRadius(new Vector2(314, 0), 200, 120);
        }
        ));


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
    Dialogue old;
    private void SetCurrentNode(int newval)
    {
        ClearOverridePortrait();
        old = currentNodeId == -1 ? null : allGameDialogue[currentNodeId];
        old?.callbackOnEnd?.Invoke();

        currentNodeId = newval;
        if (currentNodeId < 0)
        {
            //hide dialogue scene
            GD.Print("Dissabled dialogue!");
            textboxUI.Visible = false;
            OptionsUI.Visible = false;
            images.Visible = false;
            isInDialogue = false;
        }
        else
        {
            isInDialogue = true;
            var node = allGameDialogue[currentNodeId];
            //show dialogue scene
            GD.Print("progressed dialogue: id" + currentNodeId + ", \"" + allGameDialogue[currentNodeId].text + "\"");
            textbox.Text = allGameDialogue[currentNodeId].text;
            tb.DisplayText(allGameDialogue[currentNodeId].text);
            textboxUI.Visible = true;
            images.Visible = !node.hideImages;

            if (node.characterId == 0)
            {
                images.Visible = false;
            }
            else
            {
                if (overridePortrait != null)
                {
                    charImg.Texture = overridePortrait;
                }
                else
                {
                    charImg.Texture = gameCharacters[node.characterId - 1].sprite;
                }
            }

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
    }

    private void Add(Dialogue node)
    {
        allGameDialogue[node.uid] = node;
    }



}

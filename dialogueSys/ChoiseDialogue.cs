using System;
using System.Collections;

public class ChoiseDialogue : Dialogue
{
    public class Option
    {
        public string optionText;
        public int nextNodeId;
        public Action callback;

        public Option(string tx, int next, Action callb)
        {
            this.optionText = tx;
            this.nextNodeId = next;
            this.callback = callb;
        }
    }

    public Option option1;
    public Option option2;

    public ChoiseDialogue(int id, int ch, string text) : base(id, ch, text)
    {
    }

    public ChoiseDialogue Option1(Option opt)
    {
        option1 = opt;
        return this;
    }

    public ChoiseDialogue Option2(Option opt)
    {
        option2 = opt;
        return this;
    }

}
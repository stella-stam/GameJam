public class Dialogue
{
    public readonly int uid;
    public readonly int characterId;
    public readonly string text;
    private int nextNodeId = -1;

    public Dialogue(int id, int ch, string text)
    {
        uid = id;
        characterId = ch;
        this.text = text;
        nextNodeId = id + 1;
    }

    public virtual int GetNextNodeId()
    {
        return nextNodeId;
    }

    public virtual Dialogue next(int i)
    {
        nextNodeId = i;
        return this;
    }

    public virtual Dialogue end()
    {
        nextNodeId = -1;
        return this;
    }

}
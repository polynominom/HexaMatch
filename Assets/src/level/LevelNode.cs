using System;

// Binary tree sctructre that represents levels
public class LevelNode
{
    public int id;
    public LevelNode leftNode;
    public LevelNode rightNode;

    public LevelNode(int id)
    {
        this.id = id;
    }
}

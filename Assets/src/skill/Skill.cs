using System;
public abstract class Skill
{
    private int value;
    public Skill() { }

    public Skill(int value) => this.value = value;
    public int GetValue() => value;
}

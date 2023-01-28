using System;
public class RandomLevelRegister
{
    public int registeredLevel;
    public bool registeredAndActive;
    public RandomLevelRegister()
    {
        Reset();
    }

    public void Reset()
    {
        registeredAndActive = false;
        registeredLevel = -1;
    }

    public void Activate(int level)
    {
        registeredAndActive = true;
        registeredLevel = level;
    }
}

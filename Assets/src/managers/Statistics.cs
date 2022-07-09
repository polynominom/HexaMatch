using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Statistics
{
    public Statistics()
    {
        minPassedCounts = new Dictionary<int, int>();
        passedLevels = new List<int>();
        obtainedSkills = 1;
    }
    public List<int> passedLevels;
    public Dictionary<int, int> minPassedCounts;
    public int playerEssenceValue;
    public int obtainedSkills;
}

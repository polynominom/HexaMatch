using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HexaStateGoal
{
    private static System.Random rnd = new System.Random();
    private List<HexaNode> allNodes = new List<HexaNode>();
    public HexaStateGoal(List<HexaNode> allNodes)
    {
        this.allNodes = new List<HexaNode>(allNodes);
        Randomize();
    }

    private void Randomize()
    {
        var shuffled = allNodes.OrderBy(item => rnd.Next());
        var iterator = 0;
        foreach (HexaNode n in allNodes)
        {
            for (int i = 0; i < 6; i++)
            {
                if (n.neighbors[i] == null) continue;

                HexaNode newNode = shuffled.ElementAt(iterator);
                iterator++;
                n.neighbors[i] = newNode;
            }
        }
    }
}

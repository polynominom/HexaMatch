using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NodeComparer : Comparer<HexaNode>
{
    public override int Compare(HexaNode first, HexaNode second)
    {
        return first.transform.position.y.CompareTo(second.transform.position.y);
    }
}
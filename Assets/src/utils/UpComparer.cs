using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UpComparer : Comparer<Vector3>
{
    public override int Compare(Vector3 first, Vector3 second)
    {
        return first.y.CompareTo(second.y);
    }
}


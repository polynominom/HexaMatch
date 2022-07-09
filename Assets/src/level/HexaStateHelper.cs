using System.Collections.Generic;
using UnityEngine;

public class HexaStateHelper
{

    public static readonly string JsonFolderPath = "";// Application.dataPath + "/src/level/json/";
    public static readonly float precisionFactor = 0.001f;
    // neighbor ids (0,1,2,3,4,5) = (up, upRight, bottomRight, bottom, bottomLeft, upLeft)
    public static readonly Vector3 UpperLeftOffset      = new Vector3(-Mathf.Sqrt(0.75f), 0.5f, 0f);
    public static readonly Vector3 UpperRightOffset     = new Vector3(Mathf.Sqrt(0.75f), 0.5f, 0f);
    public static readonly Vector3 UpOffset             = new Vector3(0.0f, 1.0f, 0f);
    public static readonly Vector3 BottomOffset         = new Vector3(0.0f, -1.0f, 0f);
    public static readonly Vector3 BottomLeftOffset     = new Vector3(-Mathf.Sqrt(0.75f), -0.5f, 0f);
    public static readonly Vector3 BottomRightOffset    = new Vector3(Mathf.Sqrt(0.75f), -0.5f, 0f);
    public static readonly Vector3[] neighborPositions  = new Vector3[6]{UpOffset, UpperRightOffset, BottomRightOffset, BottomOffset, BottomLeftOffset, UpperLeftOffset};
    //public static readonly float[] backgroundColumnXPosArr = new float[8]{ -(Mathf.Sqrt(0.75f) * 4), -(Mathf.Sqrt(0.75f) * 3), -(Mathf.Sqrt(0.75f)*2), -Mathf.Sqrt(0.75f), 0, Mathf.Sqrt(0.75f), (Mathf.Sqrt(0.75f) * 2), (Mathf.Sqrt(0.75f) * 3) };
    public HexaStateHelper()
    {

    }

    public static bool NextState(HexaNode touchedNode)
    {
        return false;
    }

    private static bool PositionCheck(Vector3 dst, Vector3 offset)
    {
        return (dst.x - offset.x) < precisionFactor && (dst.x - offset.x) > -precisionFactor &&
            (dst.y - offset.y) < precisionFactor && (dst.y - offset.y) > -precisionFactor;
    }

    public static void ReAssignNeighbors(List<HexaNode> nodes)
    {
        foreach (HexaNode n in nodes) n.ResetNeighbors();
        // neighbor ids (0,1,2,3,4,5) = (up, upRight, bottomRight, bottom, bottomLeft, upLeft)
        for(int i = 0; i < nodes.Count; i++)
        {
            HexaNode n = nodes[i];
            for(int j = 0; j < nodes.Count; j++)
            {
                if (i == j) continue;
                HexaNode candidate = nodes[j];
                Vector3 dst = candidate.transform.position - n.transform.position;
                if(i == 5 && j == 10)
                {
                    //Debug.Log("dst: " + dst.ToString());
                }

                if (PositionCheck(dst, UpOffset))
                {
                    n.AddNeighbor(candidate, 0);
                    candidate.AddNeighbor(n, 3);
                }
                else if (PositionCheck(dst, UpperRightOffset))
                {
                    n.AddNeighbor(candidate, 1);
                    candidate.AddNeighbor(n, 4);
                }
                else if (PositionCheck(dst, BottomRightOffset))
                {
                    n.AddNeighbor(candidate, 2);
                    candidate.AddNeighbor(n, 5);
                }
                else if (PositionCheck(dst, BottomOffset))
                {
                    n.AddNeighbor(candidate, 3);
                    candidate.AddNeighbor(n, 0);
                }
                else if (PositionCheck(dst, BottomLeftOffset))
                {
                    n.AddNeighbor(candidate, 4);
                    candidate.AddNeighbor(n, 1);
                }
                else if (PositionCheck(dst, UpperLeftOffset))
                {
                    n.AddNeighbor(candidate, 5);
                    candidate.AddNeighbor(n, 2);
                } //else: ignore
            }
        }
    }

    private static HexaNode FindNode(List<HexaNode> nodes, string id)
    {
        foreach(HexaNode n in nodes)
        {
            if(n.id.ToString().Equals(id))
            {
                return n;
            }
        }
        return null;
    }
}

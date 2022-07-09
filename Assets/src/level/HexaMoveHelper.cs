using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class HexaMoveHelper
{
    private bool initialized = false;
    private HexaNode touchedNode;

    private List<Vector3> oldPositions;
    private List<Vector3> firstPositions;
    private List<HexaNode> moveableNodes;

    private NodeComparer nodeComparer = new NodeComparer();
    private UpComparer upComparer = new UpComparer();

    private MoveCorrector moveCorrector;
    private int headId;
    private int tailId;

    public HexaMoveHelper(){ }

    public void OnTouch(HexaNode node)
    {
        this.touchedNode = node;
    }

    private void AddLists(HexaNode neighbor, Queue<HexaNode> q)
    {
        if (neighbor == null)
            return;

        if (!moveableNodes.Contains(neighbor))
        {
            moveableNodes.Add(neighbor);
            oldPositions.Add(neighbor.transform.position);
            firstPositions.Add(neighbor.transform.position);
            q.Enqueue(neighbor);
        }
    }

    //Obtain the hexagon lists in one direction
    private void InitializeLists(HexaDirection direction)
    {
        oldPositions = new List<Vector3>();
        firstPositions = new List<Vector3>();
        oldPositions.Add(touchedNode.transform.position);
        firstPositions.Add(touchedNode.transform.position);
        moveableNodes = new List<HexaNode>();
        moveableNodes.Add(touchedNode);

        Queue<HexaNode> q = new Queue<HexaNode>();

        q.Enqueue(touchedNode);
        //fill moveableNodes
        while (q.Count != 0)
        {
            HexaNode n = q.Dequeue();
            if (direction == HexaDirection.up || direction == HexaDirection.bottom)
            {
                AddLists(n.neighbors[0], q);
                AddLists(n.neighbors[3], q);
                //if(n.neighbors[0] != null)
                //    Debug.Log(" - n.neighbors[0].id: " + n.neighbors[0].id.ToString());
                //if(n.neighbors[3] != null)
                //    Debug.Log(" - n.neighbors[3].id: " + n.neighbors[3].id.ToString());
            }
            else if (direction == HexaDirection.upLeft || direction == HexaDirection.bottomRight)
            {
                AddLists(n.neighbors[2], q);
                AddLists(n.neighbors[5], q);
                //if (n.neighbors[2] != null)
                //    Debug.Log(" - n.neighbors[2].id: " + n.neighbors[2].id.ToString());
                //if (n.neighbors[5] != null)
                //    Debug.Log(" - n.neighbors[5].id: " + n.neighbors[5].id.ToString());
            }
            else if (direction == HexaDirection.upRight || direction == HexaDirection.bottomLeft)
            {
                AddLists(n.neighbors[1], q);
                AddLists(n.neighbors[4], q);
                //if (n.neighbors[1] != null)
                //    Debug.Log(" - n.neighbors[1].id: " + n.neighbors[1].id.ToString());
                //if (n.neighbors[4] != null)
                //    Debug.Log(" - n.neighbors[4].id: " + n.neighbors[4].id.ToString());
            }
            //else
            //{
            //    Debug.LogError("Unexpected impossible direction");
            //}
        }

        //sort the nodes
        moveableNodes.Sort(nodeComparer);
        oldPositions.Sort(upComparer);

        HexaNode head = moveableNodes[moveableNodes.Count - 1];
        HexaNode tail = moveableNodes[0];

        headId = moveableNodes.Count - 1;
        tailId = 0;

        //head and tail handler
        moveCorrector = new MoveCorrector(moveableNodes[moveableNodes.Count - 1], moveableNodes[0], direction);
        if (direction == HexaDirection.up || direction == HexaDirection.bottom)
        {
            moveCorrector.headLimitPosition = head.transform.position + HexaStateHelper.UpOffset / 2;
            moveCorrector.tailLimitPosition = tail.transform.position + HexaStateHelper.BottomOffset / 2;
        }
        else if (direction == HexaDirection.upLeft || direction == HexaDirection.bottomRight)
        {
            moveCorrector.headLimitPosition = head.transform.position + HexaStateHelper.UpperLeftOffset / 2;
            moveCorrector.tailLimitPosition = tail.transform.position + HexaStateHelper.BottomRightOffset / 2;
        }
        else if (direction == HexaDirection.upRight || direction == HexaDirection.bottomLeft)
        {
            moveCorrector.headLimitPosition = head.transform.position + HexaStateHelper.UpperRightOffset / 2;
            moveCorrector.tailLimitPosition = tail.transform.position + HexaStateHelper.BottomLeftOffset / 2;
        }
    }

    public Vector3 GetOffset(HexaDirection direction)
    {
        Vector3 result = HexaStateHelper.UpOffset;
        if (direction == HexaDirection.upLeft || direction == HexaDirection.bottomRight)
            result = HexaStateHelper.UpperLeftOffset;
        else if (direction == HexaDirection.upRight || direction == HexaDirection.bottomLeft)
            result = HexaStateHelper.UpperRightOffset;
        return result;
    }

    public void OnDrag(Vector3 movement, HexaDirection direction)
    {
        if(!initialized)
        {
//            Debug.Log("direction: " + direction);
            InitializeLists(direction);
            initialized = true;
            if (moveableNodes == null || moveableNodes.Count <= 1)
            {
                OnDrop();
                return;
            }
        }

        for(int i = 0; i < moveableNodes.Count; i++)
            moveableNodes[i].transform.position = oldPositions[i] + movement;

        if (moveCorrector == null) return;

        int r = moveCorrector.Correct();
        if(r != 0)
        {
            if(r == 1)
            {
                // head should be on tail
                oldPositions[headId] = oldPositions[tailId] - GetOffset(direction);
                tailId = headId;
                headId = (headId - 1) % moveableNodes.Count;
                headId = headId < 0 ? headId + moveableNodes.Count : headId;
                moveCorrector.SetNodes(moveableNodes[headId], moveableNodes[tailId]);
            }
            else if(r == 2)
            {
                // tail should be on tail
                oldPositions[tailId] = oldPositions[headId] + GetOffset(direction);
                headId = tailId;
                tailId = (tailId + 1) % moveableNodes.Count;
                moveCorrector.SetNodes(moveableNodes[headId], moveableNodes[tailId]);
            }
        }

    }

    public void OnDropBefore()
    {
        List<int> handledIds = new List<int>();
        if (firstPositions != null)
        {
            foreach (Vector3 pos in firstPositions)
            {
                for (int i = 0; i < moveableNodes.Count; i++)
                {
                    if (handledIds.Contains(i)) continue;

                    // get an ordered version of moveableNodes,
                    // check if it passes
                    if (Vector3.Distance(pos, moveableNodes[i].transform.position) < 0.5)
                    {
                        moveableNodes[i].transform.position = pos;
                        handledIds.Add(i);
                    }
                }
            }
        }

        oldPositions = null;
        moveableNodes = null;
        firstPositions = null;
        initialized = false;
    }

    public void OnDrop()
    {
        if (moveableNodes == null || firstPositions == null)
            return;

        var orderedNodes = moveableNodes.OrderBy(o => o.transform.position.y).ToList();
        var firstPositionOrdered = firstPositions.OrderBy(o => o.y).ToList();
        if(firstPositions != null)
        {
            for (int i = 0; i < orderedNodes.Count; i++)
            {
                orderedNodes[i].transform.position = firstPositionOrdered[i];
            }
        }

        oldPositions = null;
        moveableNodes = null;
        firstPositions = null;
        initialized = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MoveCorrector
{
    private HexaDirection direction;
    private HexaNode head;
    private HexaNode tail;

    public Vector3 headLimitPosition;
    public Vector3 tailLimitPosition;

    private float precisionOffset = 0.01f;

    public MoveCorrector(HexaNode head, HexaNode tail, HexaDirection direction)
    {
        this.direction = direction;
        this.head = head;
        this.tail = tail;

        if (!(head&&tail))
            return;

        SetNodes(head, tail);
    }

    public void SetNodes(HexaNode head, HexaNode tail)
    {
        this.head = head;
        this.tail = tail;

    }

    public int Correct()
    {
        int result = 0;
        if (direction == HexaDirection.up || direction == HexaDirection.bottom)
        {
            if(head.transform.position.y > headLimitPosition.y)
            {
                // head reached limit
                //head.transform.position = tailLimitPosition;
                result = 1;
            }
            if(tail.transform.position.y < tailLimitPosition.y)
            {
                // tail reached limit
                //tail.transform.position = headLimitPosition;
                result = 2;
            }
        }

        else if (direction == HexaDirection.upLeft || direction == HexaDirection.bottomRight)
        {
            if (head.transform.position.y > headLimitPosition.y &&
                head.transform.position.x < headLimitPosition.x)
            {
                //head.transform.position = tailLimitPosition;
                result = 1;
            }

            if (tail.transform.position.y < tailLimitPosition.y &&
                tail.transform.position.x > tailLimitPosition.x)
            {
                //tail.transform.position = headLimitPosition;
                result = 2;
            }
        }
        else if (direction == HexaDirection.upRight || direction == HexaDirection.bottomLeft)
        {
            if (head.transform.position.y > headLimitPosition.y &&
                head.transform.position.x > headLimitPosition.x)
            {
                //head.transform.position = tailLimitPosition;
                result = 1;
            }

            if (tail.transform.position.y < tailLimitPosition.y &&
                tail.transform.position.x < tailLimitPosition.x)
            {
                //tail.transform.position = headLimitPosition;
                result = 2;
            }
        }

        return result;
    }
}
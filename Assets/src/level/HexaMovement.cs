using UnityEngine;

public class HexaMovement : MonoBehaviour
{
    public Vector3 mOffset;
    public Vector3 oldPos;
    public HexaDirection movementDir = HexaDirection.up;
    private HexaMoveHelper hexaMoveHelper = null;

    private bool movementLock = false;

    private Vector3 FindDriection(Vector3 from, Vector3 to)
    {
        //UpOffset, UpperRightOffset, BottomRightOffset, BottomOffset, BottomLeftOffset, UpperLeftOffset
        Vector3 diff = to - from;
        float magnitude = Vector3.Magnitude(diff);
        if (magnitude < 0.2)
            return Vector3.zero;


        float angle1 = Vector3.Angle(HexaStateHelper.UpOffset, diff);
        float angle2 = Vector3.Angle(HexaStateHelper.UpperRightOffset, diff);
        float angle3 = Vector3.Angle(HexaStateHelper.BottomRightOffset, diff);
        float angle4 = Vector3.Angle(HexaStateHelper.BottomOffset, diff);
        float angle5 = Vector3.Angle(HexaStateHelper.BottomLeftOffset, diff);
        float angle6 = Vector3.Angle(HexaStateHelper.UpperLeftOffset, diff);
        float angleLimit = 45;

        if (movementLock)
        {
            if (angle1 < angleLimit && (movementDir == HexaDirection.up
                                    || movementDir == HexaDirection.bottom))
            {
                movementDir = HexaDirection.up;
            }
            else if (angle2 < angleLimit && (movementDir == HexaDirection.upRight
                                    || movementDir == HexaDirection.bottomLeft))
            {
                movementDir = HexaDirection.upRight;
            }
            else if (angle3 < angleLimit && (movementDir == HexaDirection.bottomRight
                                    || movementDir == HexaDirection.upLeft))
            {
                movementDir = HexaDirection.bottomRight;
            }
            else if (angle4 < angleLimit && (movementDir == HexaDirection.up
                                    || movementDir == HexaDirection.bottom))
            {
                movementDir = HexaDirection.bottom;
            }
            else if (angle5 < angleLimit && (movementDir == HexaDirection.upRight
                                    || movementDir == HexaDirection.bottomLeft))
            {
                movementDir = HexaDirection.bottomLeft;
            }
            else if (angle6 < angleLimit && (movementDir == HexaDirection.bottomRight
                                    || movementDir == HexaDirection.upLeft))
            {
                movementDir = HexaDirection.upLeft;
            }
            //else return Vector3.zero;
        }
        else
        {
            if (angle1 < angleLimit)
            {
                movementLock = true;
                movementDir = HexaDirection.up;
            }
            else if (angle2 < angleLimit)
            {
                movementLock = true;
                movementDir = HexaDirection.upRight;
            }
            else if (angle3 < angleLimit)
            {
                movementLock = true;
                movementDir = HexaDirection.bottomRight;
            }
            else if (angle4 < angleLimit)
            {
                movementLock = true;
                movementDir = HexaDirection.bottom;
            }
            else if (angle5 < angleLimit)
            {
                movementLock = true;
                movementDir = HexaDirection.bottomLeft;
            }
            else if (angle6 < angleLimit)
            {
                movementLock = true;
                movementDir = HexaDirection.upLeft;
            }
        }

        return HexaStateHelper.neighborPositions[(int)movementDir - 1] * magnitude;
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = 0;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    protected void OnMouseDown()
    {
        if (GameManager.IsLevelEnded())
            return;

        // skill perform
        if (SkillManager.Instance.IsSkillActive() )
        {
            // SELECTED HEXAGON REGISTERED INTO SKILL MANGAER
            SkillManager.Instance.OnHexagon(GetComponent<HexaNode>());
        }
        else
        {
            // normal functionality without skills
            oldPos = transform.position;
            mOffset = transform.position - GetMouseWorldPos();

            hexaMoveHelper = new HexaMoveHelper();
            hexaMoveHelper.OnTouch(GetComponent<HexaNode>());
        }
    }

    protected void OnMouseDrag()
    {
        if (GameManager.IsLevelEnded())
            return;

        Vector3 dir = FindDriection(oldPos, GetMouseWorldPos() + mOffset);
        if(movementLock && hexaMoveHelper != null)
            hexaMoveHelper.OnDrag(dir, movementDir);
    }

    protected void OnMouseUp()
    {
        movementLock = false;
        //   transform.position = oldPos;
        if (hexaMoveHelper != null)
        {
            hexaMoveHelper.OnDrop();
            hexaMoveHelper = null;

            //re-neighboring
            var state = GetComponentInParent<HexaState>();
            if(state != null)
                HexaStateHelper.ReAssignNeighbors(state.GetAllNodes());
        }

        //Goal checking
        GameManager.CheckLevelFinished();
    }
}

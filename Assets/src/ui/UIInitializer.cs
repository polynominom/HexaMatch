using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInitializer : MonoBehaviour
{
    public Transform buttonTargetPlaceholder;
    public Transform buttonParent;

    private void ChangeOrderInLayer(Transform level, int newOrderId)
    {
        for (int i = 0; i < level.childCount; i++)
        {
            Transform c = level.GetChild(i);
            c.GetComponent<SpriteRenderer>().sortingOrder = newOrderId;
            c.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = newOrderId;
        }
    }


    private void InitTargetArea()
    {
        GameObject startinglevel = GameObject.FindGameObjectWithTag("Level");
        GameObject level = GameObject.FindGameObjectWithTag("Goal");
        if (level == null)
            return;

        Transform levelClone = Instantiate(level.transform, buttonParent);
        // little target area
        levelClone.transform.parent = buttonParent;
        levelClone.localScale = buttonTargetPlaceholder.localScale;
        levelClone.position = buttonTargetPlaceholder.position;
    }

    private void Start()
    {
        InitTargetArea();
    }
}

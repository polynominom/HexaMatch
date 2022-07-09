using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenLevelsShade : MonoBehaviour
{
    public List<Transform> shadowedLevels;
    public List<int> constrainingIds;
    void Start()
    {
        bool isShadeOff = LevelManager.Instance.AreLevelsPassed(constrainingIds);
        gameObject.SetActive(!isShadeOff);
        foreach(var t in shadowedLevels)
            t.GetComponent<PolygonCollider2D>().enabled = isShadeOff;
    }

}

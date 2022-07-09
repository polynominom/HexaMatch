using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum LevelType
{
    Normal,
    Bonus,
    ExtraBonus,
    AdditionalExtraBonus,
    Hidden
}

public class LevelNodeUI: MonoBehaviour
{


    private int id;
    public LevelType levelType;
    public int ReferencedLevelId;

    private bool passed;
    public void SetId(int id)
    {
        this.id = id;
        this.passed = false;
    }

    public int GetId()
    {
        return id;
    }

    public void SetPassed(bool passed)
    {
        this.passed = passed;
    }

    public void ApplyPassedColor(Material mat)
    {
        ApplyColor(mat);
    }

    private void ApplyColor(Material mat)
    {
        if (mat == null)
            return;

        GetComponent<SpriteRenderer>().material = mat;
    }

    private void OnMouseDown()
    {
        int sceneId = 0;
        if (ReferencedLevelId >= 1)
            sceneId = LevelManager.Instance.GetSceneIdFromLevelId(ReferencedLevelId);

        GameManager.Instance.LoadScene(sceneId);
    }

}
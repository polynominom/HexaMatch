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
        if (ReferencedLevelId >= 1 && ReferencedLevelId <= 23)
        {
            sceneId = LevelManager.Instance.GetSceneIdFromLevelId(ReferencedLevelId);
        }
        else // ReferencedLevelId > 23
        {
            // Manually assign it scene id random level generator
            if (ReferencedLevelId < 30)
                sceneId = Constants.ID_EASY_RANDOM_LEVEL;
            else if (ReferencedLevelId < 40)
                sceneId = Constants.ID_MEDIUM_RANDOM_LEVEL;
            else
                sceneId = Constants.ID_HARD_RANDOM_LEVEL;

            // Track the referenced level id in random level
            GameManager.Instance.randomLevelRegister.Activate(ReferencedLevelId);
        }
        
        GameManager.Instance.LoadScene(sceneId);
    }

}
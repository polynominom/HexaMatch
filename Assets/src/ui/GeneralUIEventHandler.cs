using UnityEngine;

public class GeneralUIEventHandler : MonoBehaviour
{
    public void OnContinue()
    {
        if(LevelManager.Instance.IsLevelRandom())
        {
            GameManager.Instance.Restart();
            return;
        }

        int nextLevel = LevelManager.Instance.GetNextLevel();
        int loadLevel = LevelManager.Instance.GetSceneIdFromLevelId(nextLevel);
        if (nextLevel == Constants.ID_LEVELS)
            loadLevel = Constants.ID_LEVELS;

        GameManager.Instance.LoadScene(loadLevel);
    }

    public void OnHome()
    {
        GameManager.Instance.LoadScene(Constants.ID_HOME);
    }

    public void OnRestart()
    {
        // Handle Random State
        GameManager.Instance.Restart();
        //GameManager.Instance.LoadScene(LevelManager.Instance.GetLevelID() + Constants.ID_FIRST_LEVEL);
    }

    public void OnLevels()
    {
        GameManager.Instance.LoadScene(Constants.ID_LEVELS);
    }

    /// <summary>
    /// Tries to activate skills and make user be able to use a skill.
    ///  - note: currently only swap skill is used. for later it should be more generic.
    /// </summary>
    public void OnSkill(GameObject buttonObject)
    {
        // Idia: etting swap skill is hardcoded, might be good idea to change it
        //          to more generic approach
        if(SkillManager.Instance.CanAfford())
        {
            if (!SkillManager.Instance.IsSkillActive())
                SkillManager.Instance.RegisterSkill(new SwapSkill());
            else
                SkillManager.Instance.CleanUp();
        }
        else
        {
            var menu = SkillManager.Instance.GetSkillSlideMenu();
            menu.Activate(buttonObject);
        }

    }
}

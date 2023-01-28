using System.Collections.Generic;

public class LevelManager: Singleton<LevelManager>
{
    public bool LevelPassedBefore = false;
    private HexaState activeState;
    private Dictionary<int, int> LevelIdSceneIdMap;

    protected LevelManager()
    {
        LevelIdSceneIdMap = new Dictionary<int, int>();
        for(int i = 1; i <= Constants.LevelCount; ++i)
            LevelIdSceneIdMap.Add(i, i+1);
    }

    public void SetActiveLevel(HexaState activeState)
    {
        this.activeState = activeState;
        var st = DataManager.Instance.LoadStatistics();
        LevelPassedBefore = st.passedLevels.Contains(activeState.LevelId);
    }

    public HexaState GetActiveState()
    {
        return activeState;
    }

    public List<float> GetLevelEssence()
    {
        var result = new List<float>();
        if(activeState.IsRandom || !LevelPassedBefore)
        {
            result.Add(activeState.LevelUnclaimedValue);
            result.Add(2);
        }
        else
        {
            result.Add(activeState.LevelClaimedValue);
            result.Add(0.5F);
        }

        return result;
    }

    public bool IsLevelRandom()
    {
        return activeState != null && activeState.IsRandom;
    }

    // return next levelId as in Level ID (not Scene ID).
    public int GetNextLevel()
    {
        int levelId = GetLevelID();
        if(levelId+1 <= Constants.LevelCount+1)
        {
            return levelId + 1;
        }
        return Constants.ID_LEVELS;
    }

    // Updates the persisted passed levels if not random
    // Updates the essence
    public void LevelPassed(int id, bool isLevelRandom = false)
    {
        if (activeState == null)
            return;

        var st = DataManager.Instance.LoadStatistics();
        
        if (!activeState.IsRandom)
        {
            // add the passed level to the persistance list
            if (!st.passedLevels.Contains(activeState.LevelId))
            {
                st.passedLevels.Add(activeState.LevelId);
                st.playerEssenceValue += activeState.LevelUnclaimedValue;
            }
            else
            {
                st.playerEssenceValue += activeState.LevelClaimedValue;
            }
        }
        else
        {
            if(GameManager.Instance.randomLevelRegister.registeredAndActive)
            {
                // random state but clicked from levels
                var a = GameManager.Instance.randomLevelRegister.registeredLevel;
                if ( !st.passedLevels.Contains(a) )
                {
                    st.passedLevels.Add(a);
                    st.playerEssenceValue += activeState.LevelUnclaimedValue;
                }
                else
                {
                    st.playerEssenceValue += activeState.LevelClaimedValue;
                }
            }

            st.playerEssenceValue += activeState.LevelUnclaimedValue;
        }
            

        // update value in game manager
        GameManager.Instance.PlayerEssence = st.playerEssenceValue;
        DataManager.Instance.SaveStatistics(st);
    }

    public int GetLevelID()
    {
        return GameManager.Instance.GetCurrentScene() - Constants.ID_FIRST_LEVEL + 1;
    }

    public int GetSceneIdFromLevelId(int levelId)
    {
        if (levelId <= Constants.LevelCount)
            return levelId + 1;
        else if (levelId == Constants.LevelCount + 1)
            return Constants.ID_LEVELS;
        else
            return Constants.ID_HOME;
    }

    // Tries to find if the given levels are already passed by the user.
    public bool AreLevelsPassed(List<int> levelIds)
    {
        var st = DataManager.Instance.LoadStatistics();
        int result = 0;
        foreach (int id in levelIds)
            if (st.passedLevels.Contains(id))
                result += 1;

        return result == levelIds.Count;
    }
}

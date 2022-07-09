using System.Collections.Generic;

public class LevelManager: Singleton<LevelManager>
{
    public bool LevelPassedBefore = false;
    private HexaState activeState;
    private Dictionary<int, int> LevelIdSceneIdMap;
    // levelIds that are allowed to be passed
    private List<int> commonLevelIds;
    private List<int> hiddenLevels1;
    private List<int> hiddenLevels2;

    protected LevelManager()
    {
        commonLevelIds = new List<int>() {
            1,2,4,5,6,7,9,10,12,13,14
        };

        hiddenLevels1 = new List<int>() {
            21,22,23
        };

        hiddenLevels2 = new List<int>() {
            18,19,20
        };

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
        for(int i = 0; i < commonLevelIds.Count - 1; ++i)
        {
            if (commonLevelIds[i] == levelId)
                return commonLevelIds[i + 1];
        }

        if (commonLevelIds[commonLevelIds.Count - 1] == levelId)
            return Constants.ID_LEVELS;

        for(int i = 0; i < hiddenLevels1.Count - 1; ++i)
        {
            if (hiddenLevels1[i] == levelId)
                return hiddenLevels1[i + 1];
        }

        if (hiddenLevels1[hiddenLevels1.Count - 1] == levelId)
            return Constants.ID_LEVELS;

        for (int i = 0; i < hiddenLevels2.Count - 1; ++i)
        {
            if (hiddenLevels2[i] == levelId)
                return hiddenLevels2[i + 1];
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
        if (LevelIdSceneIdMap.ContainsKey(levelId))
            return LevelIdSceneIdMap[levelId];
        else
            return 0;
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

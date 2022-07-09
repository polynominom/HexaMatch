using UnityEngine;
using System.IO;
using System.Threading.Tasks;

public class DataManager : Singleton<DataManager>
{
    private Statistics statistics = null;
    private string pDataPath;
    private void Awake()
    {
        pDataPath = Application.persistentDataPath;
    }
   

    // saving current level data to continue
    public void SaveGameData(GameData gameData)
    {
        string dataAsJson = JsonUtility.ToJson(gameData);
        File.WriteAllText(GetGameDataPath(), dataAsJson);
    }

    // load the level data
    public GameData LoadGameData()
    {
        if(File.Exists(GetGameDataPath()))
        {
            string dataAsJson = File.ReadAllText(GetGameDataPath());
            GameData gd = JsonUtility.FromJson<GameData>(dataAsJson);
            return gd;
        }

        return null;
    }

    public void SaveEssence(int newEssence)
    {
        var st = LoadStatistics();
        st.playerEssenceValue = newEssence;
        SaveStatistics(st);
    }

    public void SaveObtainedSkills(int obtainedSkills)
    {
        var st = LoadStatistics();
        st.obtainedSkills = obtainedSkills;
        SaveStatistics(st);
    }

    private string GetGameDataPath()
    {
        return pDataPath + Constants.GAME_DATA_SAVE_LOCATION;
    }

    private string GetStatisticsPath()
    {
        return pDataPath + Constants.STATISTICS_SAVE_LOCATION;
    }

    public void SaveStatistics(Statistics st)
    {
        //save file in a task that is running under another thread.
        Task.Run(() =>
        {
            string dataAsJson = JsonUtility.ToJson(statistics);
            File.WriteAllText(GetStatisticsPath(), dataAsJson);
        });
    }

    public void PersistData()
    {
        if(statistics != null)
        {
            string dataAsJson = JsonUtility.ToJson(statistics);
            File.WriteAllText(GetStatisticsPath(), dataAsJson);
        }
    }

    // lazy laods statistics
    public Statistics LoadStatistics()
    {
        if (statistics != null)
            return statistics;

        // LOAD ONCE
        if(File.Exists(GetStatisticsPath()))
        {
            string dataAsJson = File.ReadAllText(GetStatisticsPath());
            this.statistics = JsonUtility.FromJson<Statistics>(dataAsJson);
        }
        else
        {
            statistics = new Statistics();
        }
            
        return statistics;
    }

    // REMOVE FOR RELEASE
    public void DeleteStats()
    {
        SaveStatistics(new Statistics());
    }

    // Saving passed levels are handled here.
    public void SavePassedLevelIndex(int index, int tryCount)
    {
        Statistics st = LoadStatistics();

        // add index if it doesn't already exists
        if(!st.passedLevels.Contains(index))
            st.passedLevels.Add(index);

        // handle the passed count
        // fixme: find a better way to write this code
        if(st.minPassedCounts.ContainsKey(index))
        {
            if(tryCount < st.minPassedCounts[index])
            {
                st.minPassedCounts[index] = tryCount;
            }
        }
        else
        {
            st.minPassedCounts[index] = tryCount;
        }

        SaveStatistics(st);
    }
}

using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    // will be activated on extra levels
    public RandomLevelRegister randomLevelRegister;

    private bool LevelEnded = false;
    private AnimationHandler animationHandler = null;

    public event Action EssenceChange;

    public int PlayerEssence
    {
        get
        {
            return m_PlayerEssence;
        }
        set
        {
            m_PlayerEssence = value;
            if (EssenceChange != null)
                EssenceChange();
            //else
            //    Debug.Log("ERROR!! UNABLE TO BROADCAST DUE TO INACTIVITY!");
        }
    }
    private int m_PlayerEssence;

    private void Awake()
    {
        m_PlayerEssence = DataManager.Instance.LoadStatistics().playerEssenceValue;
    }
    //private TryCountListener tryCounterListener = null;
    //private int TryCount = 0;

    // Use this for initialization
    // (Optional) Prevent non-singleton constructor use.
    protected GameManager()
    {
        PlayerEssence = 0;
        randomLevelRegister = new RandomLevelRegister();
    }

    public static bool IsLevelEnded()
    {
        return Instance.LevelEnded;
    }

    // Function that calcutates the new essence value and persists it.
    // param:
    //      essence: should be the updated essence value. NOT THE NEW CALCULATED VALUE
    public static void UpdateEssence(int changedEssenceValue)
    {
        Instance.PlayerEssence += changedEssenceValue;
        DataManager.Instance.SaveEssence(Instance.PlayerEssence);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit ();
#endif
    }

    public static void ResetEventListeners()
    {
        // Reset the event listeners!
        Instance.EssenceChange = () => { };
        SkillManager.Instance.ResetSkillEventListeners();
    }

    public static void CheckLevelFinished()
    {
        Instance.LevelEnded = EndGameLogic.Instance.IsGoalReched();
        if ( Instance.LevelEnded)
            FinishLevel();

    }

    public static void FinishLevel()
    {
        EndGameLogic.Instance.StartLevelEndParticles();
        if (Instance.animationHandler == null)
            Instance.animationHandler = GameObject.FindGameObjectWithTag("GeneralUI").GetComponent<AnimationHandler>();

        Instance.animationHandler.OnLevelEnd();
        LevelManager.Instance.LevelPassed(Instance.GetCurrentScene());
        //Instance.randomLevelRegister.Reset();
    }

    private void LoadSceneByIndex(int sceneIndex)
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        if (sceneIndex >= sceneCount)
            sceneIndex -= 1;

        LevelEnded = false;
        //Reset the event listeners just in case
        ResetEventListeners();

        // Load the scene
        SceneManager.LoadScene(sceneIndex);
    }

    public void Continue()
    {
        GameData gd = DataManager.Instance.LoadGameData();
        int savedIndex = Constants.ID_FIRST_LEVEL;
        if (gd != null && gd.Level >= Constants.ID_FIRST_LEVEL)
            savedIndex = gd.Level;
        //else
        //{
        //    Debug.LogError("Game data is null. There is no level saved!");
        //}

        LoadSceneByIndex(savedIndex);
    }

    public int GetCurrentScene()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public void LevelsScene()
    {
        LoadSceneByIndex(Constants.ID_LEVELS);
    }

    public void Home()
    {
        LoadSceneByIndex(Constants.ID_HOME);
    }

    public void Restart()
    {
        LoadSceneByIndex(GetCurrentScene());
    }

    public void LoadScene(int index)
    {
        LoadSceneByIndex(index);
    }

    public void OnDestroy()
    {
        //DataManager.Instance.PersistData();
    }
}

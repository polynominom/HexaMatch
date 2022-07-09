using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeUIEventHandler : MonoBehaviour
{
    public void OnPlay()
    {
        GameManager.Instance.Continue();
    }

    public void OnLevels()
    {
        GameManager.Instance.LoadScene(Constants.ID_LEVELS);
    }

    public void OnStore()
    {
    }

    public void OnInfo()
    {

    }

    public void OnEasy()
    {
        GameManager.Instance.LoadScene(Constants.ID_EASY_RANDOM_LEVEL);
    }

    public void OnMedium()
    {
        GameManager.Instance.LoadScene(Constants.ID_MEDIUM_RANDOM_LEVEL);
    }

    public void OnHard()
    {
        GameManager.Instance.LoadScene(Constants.ID_HARD_RANDOM_LEVEL);
    }
}

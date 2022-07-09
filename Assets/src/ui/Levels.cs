using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Levels: MonoBehaviour
{
    public Material passedMat;
    private int debugLevelIndex = 1;
    void Start()
    {
        OpenLevels();
    }

    private void OpenLevels()
    {
        Statistics st = DataManager.Instance.LoadStatistics();
        List<int> s = new List<int>();
        if (st != null)
        {
            s = st.passedLevels;
        }

        int n = transform.GetChild(0).childCount;
        for(int i = 1; i < n-4; i++)
        {
            LevelNodeUI l = transform.GetChild(0).GetChild(i).GetComponent<LevelNodeUI>();
            bool hasPassed = s.Contains( l.ReferencedLevelId );

            if (l == null) continue;
            l.SetId(i);
            l.SetPassed(hasPassed);
            if( hasPassed )
                l.ApplyPassedColor(passedMat);
        }
    }

    public void OnHome()
    {
        GameManager.Instance.LoadScene(Constants.ID_HOME);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            var st = DataManager.Instance.LoadStatistics();
            debugLevelIndex = st.passedLevels.Count >= 1 ? st.passedLevels[st.passedLevels.Count - 1] : 0;
            //Debug.Log("passing " + debugLevelIndex+1);
            st.passedLevels.Add(debugLevelIndex+1);
            DataManager.Instance.SaveStatistics(st);
            GameManager.Instance.Restart();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            var st = DataManager.Instance.LoadStatistics();
            st.passedLevels.Clear();
            DataManager.Instance.SaveStatistics(st);
            GameManager.Instance.Restart();
        }
    }
}

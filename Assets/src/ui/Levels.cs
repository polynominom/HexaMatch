using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Levels: MonoBehaviour
{
    public Material passedMat;
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

        var content = transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(0);
        int n = content.childCount;
        for(int i = 0; i < n; i++)
        {
            LevelNodeUI l = content.GetChild(i).GetComponent<LevelNodeUI>();
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
}

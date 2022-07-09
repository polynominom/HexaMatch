using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCountDisplayer : MonoBehaviour
{
    Text text;

    public void Awake()
    {
        SkillManager.Instance.SkillCountChange += OnSkillCountChange;
        OnSkillCountChange();
    }

    public void OnSkillCountChange()
    {
        GetComponent<Text>().text = string.Format("x {0}", SkillManager.Instance.GetObtainedSkills().ToString());
    }

    //private void OnDestroy()
    //{
    //    SkillManager.Instance.SkillCountChange -= OnSkillCountChange;
    //}
}

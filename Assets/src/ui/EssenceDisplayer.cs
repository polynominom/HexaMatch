using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EssenceDisplayer : MonoBehaviour
{
    public bool WithCross;
    void Start()
    {
        GameManager.Instance.EssenceChange += OnEssenceChange;
        string essenceStr;
        if (WithCross)
            essenceStr = string.Format("x {0}", GameManager.Instance.PlayerEssence);
        else
            essenceStr = string.Format("{0}", GameManager.Instance.PlayerEssence);
        GetComponent<Text>().text = essenceStr;
    }

    public void OnEssenceChange()
    {
        string essenceStr;
        if (WithCross)
            essenceStr = string.Format("x {0}", GameManager.Instance.PlayerEssence);
        else
            essenceStr = string.Format("{0}", GameManager.Instance.PlayerEssence);
        if(gameObject != null)
            GetComponent<Text>().text = essenceStr;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexaBackground : MonoBehaviour
{
    void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Transform c = transform.GetChild(i);
            c.GetComponent<SpriteRenderer>().enabled = false;
            c.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
        }
    }
}

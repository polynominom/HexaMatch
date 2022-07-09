using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGHexaCell : MonoBehaviour
{
    private void OnMouseDown()
    {
        print(transform.position);

        //GameObject go = GameObject.FindGameObjectWithTag("LevelGeneratorWithMouse");
        //go.GetComponent<LevelGeneratorWithMouse>().AddHexa(transform.position);
    }
}

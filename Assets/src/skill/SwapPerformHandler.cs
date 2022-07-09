using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class SwapPerformHandler : SkillPerformHandler
{
    private HexaNode n1 = null;
    private HexaNode n2 = null;

    /// <summary>
    /// return 1 => node 1 set
    /// return 2 => node 2 set
    /// return 3 => node 1 is selected again so reset the selection
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    public int OnHexaSelect(HexaNode n)
    {
        if (n1 == null)
        {
            n1 = n;
            return 1;
        }

        if(n1 == n)
            return 3;

        if (n2 == null)
        {
            n2 = n;
            shouldPerform = true;
            return 2;
        }

        //if(n1 != null && n2!= null)
        //    Debug.Log("Unexpected condition: 2 hexanodes already set call perform to perform or cancel to cancel the perform");

        return -1;
    }

    public HexaNode GetHexa(int i)
    {
        if (i == 0) return n1;
        else return n2;
    }

    public void SwapMaterials()
    {
        if (n1 == null || n2 == null)
            return;

        Material tmp = n1.GetComponent<SpriteRenderer>().material;
        n1.GetComponent<SpriteRenderer>().material = n2.GetComponent<SpriteRenderer>().material;
        n2.GetComponent<SpriteRenderer>().material = tmp;
    }

    public void  SwapTypes()
    {
        if (n1 == null || n2 == null) return;

        HexaType tmp = n1.type;
        n1.type = n2.type;
        n2.type = tmp;
    }
}

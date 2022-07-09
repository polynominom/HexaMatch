using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexaNode: MonoBehaviour
{
    public int id;
    public HexaType type;
    public HexaNode[] neighbors = new HexaNode[6]{ null, null, null, null, null, null};

    private void Start()
    {
        if ( transform.childCount > 1 )
        {
            //transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public HexaNode()
    {
    }

    public HexaNode(HexaType type)
    {
        this.type = type;
    }


    public void ResetNeighbors()
    {
        neighbors = new HexaNode[6] { null, null, null, null, null, null };
    }

    public void AddNeighbor(HexaNode node, int neighborId)
    {
        if (neighbors.Length == 0)
            neighbors = new HexaNode[6] { null, null, null, null, null, null };
        neighbors[neighborId] = node;
    }
    /**
     *  Puts the neighbor id into the node
     * */
    public void AddNeighborReverse(HexaNode node, int neighborId)
    {
        if (neighbors.Length == 0)
            neighbors = new HexaNode[6] { null, null, null, null, null, null };

        int id = neighborId + 3;
        if (neighborId >= 3)
        {
            id = neighborId - 3;
        }

        neighbors[id] = node;
    }

    public void SetColor(HexaType type, List<Material> materials)
    {
        this.type = type;
        Material material = materials[0];
        switch(type)
        {
            case HexaType.red:
                material = materials[0];
                break;
            case HexaType.black:
                material = materials[1];
                break;
            case HexaType.blue:
                material = materials[2];
                break;
            case HexaType.purple:
                material = materials[3];
                break;
            case HexaType.yellow:
                material = materials[4];
                break;
            case HexaType.green:
                material = materials[5];
                break;
            case HexaType.white:
                material = materials[6];
                break;
            case HexaType.empty:
                material = materials[7];
                break;
        }
        transform.GetComponent<SpriteRenderer>().material = material;
    }

    // also sets type
    public void SetColor(string color, List<Material> materials)
    {
        if (color.Equals(HexaType.red.ToString()))
        {
            transform.GetComponent<SpriteRenderer>().material = materials[0];
            type = HexaType.red;
        }
        else if (color.Equals(HexaType.black.ToString()))
        {
            transform.GetComponent<SpriteRenderer>().material = materials[1];
            type = HexaType.black;
        }
        else if (color.Equals(HexaType.blue.ToString()))
        {
            //default is blue
            type = HexaType.blue;
        }
        else if (color.Equals(HexaType.purple.ToString()))
        {
            transform.GetComponent<SpriteRenderer>().material = materials[3];
            type = HexaType.purple;
        }
        else if (color.Equals(HexaType.yellow.ToString()))
        {
            transform.GetComponent<SpriteRenderer>().material = materials[4];
            type = HexaType.yellow;
        }
        else if (color.Equals(HexaType.green.ToString()))
        {
            transform.GetComponent<SpriteRenderer>().material = materials[5];
            type = HexaType.green;
        }
        else if (color.Equals(HexaType.white.ToString()))
        {
            transform.GetComponent<SpriteRenderer>().material = materials[6];
            type = HexaType.white;
        }
        else if (color.Equals(HexaType.empty.ToString()))
        {
            transform.GetComponent<SpriteRenderer>().material = materials[7];
            type = HexaType.empty;
        }
        else
        {
            //Debug.Log("unable to detect the color id: " + color);
            //Debug.Log("extravaganze: " + HexaType.blue.ToString());
        }
        //Material m = transform.GetComponent<SpriteRenderer>().material;
    }
}
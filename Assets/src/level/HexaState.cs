using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System;

public class HexaState: MonoBehaviour
{
    private bool isRandom = false;
    public bool IsRandom
    {
        get
        {
            return isRandom;
        }
        set
        {
            isRandom = value;
        }
    }

    // list that keeps current nodes on the screen
    private List<HexaNode> allNodes = new List<HexaNode>();

    public int LevelId = -1;

    //public GameObject background;
    public HexaNode h;
    public List<Material> materials;

    public int LevelUnclaimedValue = -1;
    public int LevelClaimedValue
    {
        get
        {
            return LevelUnclaimedValue / 10;
        }
    }

    public void SetAllNodes(List<HexaNode> nodes)
    {
        this.allNodes = nodes;
        LevelUnclaimedValue = 10 * nodes.Count;
        HexaStateHelper.ReAssignNeighbors(allNodes);
    }

    private void Awake()
    {
        LevelManager.Instance.SetActiveLevel(this);
        //Fill allnodes
        for (int i = 0; i < transform.childCount; i++)
            allNodes.Add(transform.GetChild(i).GetComponent<HexaNode>());

        HexaStateHelper.ReAssignNeighbors(allNodes);
    }

    public List<HexaNode> GetAllNodes()
    {
        return this.allNodes;
    }

    private void Start()
    {
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.N))
        {
            GameManager.FinishLevel();
        }
    }
}

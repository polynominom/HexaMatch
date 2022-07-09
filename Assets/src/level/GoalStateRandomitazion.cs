using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;


public class GoalStateRandomitazion : MonoBehaviour
{
    private static System.Random randomizer = new System.Random();

    public List<Material> materials;
    private List<Vector3> locals = new List<Vector3>();
    private List<int> types = new List<int>();

    private List<HexaNode> nodes = new List<HexaNode>();

    public void Awake()
    {
    }

    private void Update()
    {
    }

    public void SetAllNodes(List<HexaNode> nodes)
    {
        this.nodes = nodes;
        ApplyRandomization();
        HexaStateHelper.ReAssignNeighbors(nodes);
    }


    // randomizes the types
    // re-establish the neighbors to levelover check
    private void ApplyRandomization()
    {
        List<HexaType> types = new List<HexaType>();

        nodes.ForEach((HexaNode node) =>
        {
            locals.Add(node.transform.localPosition);
            types.Add(node.type);
        });


        types = types.OrderBy(item => randomizer.Next()).ToList();


        for (int i = 0; i < nodes.Count; i++)
            nodes[i].SetColor(types[i], materials);

    }
}

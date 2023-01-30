using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EndGameLogic: Singleton<EndGameLogic>
{
    private GameObject Level = null;
    private GameObject GoalState = null;
    private List<HexaNode> GoalNodes;
    private ParticleSystem _particleSystem;
    protected EndGameLogic()
    {

    }

    private void Awake()
    {

    }

    private void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        Level = GameObject.FindGameObjectWithTag("Level");
        GoalState = GameObject.FindGameObjectWithTag("Goal");
        if (GoalState == null)
            return;

        GoalNodes = new List<HexaNode>();
        int limit = GoalState.transform.childCount;
        for (int i = 0; i < limit; i++)
        {
            GoalNodes.Add(GoalState.transform.GetChild(i).GetComponent<HexaNode>());
        }

        // HexaStateHelper.ReAssignNeighbors(GoalNodes);
    }

    private HexaNode GetTargetNodeByPos(Vector3 pos)
    {
        if (GoalState == null)
            return null;

        foreach (HexaNode n in GoalNodes)
        {
            float dst = Vector3.Distance(pos, n.transform.localPosition);
            if ( dst < 0.01 )
            {
                return n;
            }
        }

        return null;
    }

    public bool IsGoalReched()
    {
        if ( GoalNodes == null || Level == null )
            return false;

        foreach ( HexaNode n in Level.transform.GetComponent<HexaState>().GetAllNodes() )
        {
            HexaNode goalNodeInPosition = GetTargetNodeByPos( n.transform.localPosition );
            if ( goalNodeInPosition.type != n.type )
                return false;
        }
        return true;
    }

    public void StartLevelEndParticles()
    {
        // Every hexanode is same = it is the end so start particle system.
        if (_particleSystem != null)
            _particleSystem.Play();
    }
}

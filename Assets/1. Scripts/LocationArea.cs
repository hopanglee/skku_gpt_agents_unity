using System;
using System.Collections.Generic;
using UnityEngine;

public class LocationArea : MonoBehaviour
{
    public TerrainManager.LocationTag location;

    [SerializeField]
    private HashSet<Agent> agents = new();

    [SerializeField]
    private Transform position;

    private void Awake()
    {
        TerrainManager.AddLocation(
            location,
            new TerrainManager.LocationInfo { transform = position, area = this }
        );
    }

    public void Enter(Agent agent)
    {
        agents.Add(agent);
    }

    public void Exit(Agent agent)
    {
        agents.Remove(agent);
    }

    public HashSet<Agent> GetAgents()
    {
        return agents;
    }

    public void MsgMediator(Agent sender, string str, float m_volume)
    {
        foreach (var _agent in agents)
        {
            if (_agent != sender && sender.Distance(_agent) <= m_volume)
            {
                _agent.OnEventListener(sender, str);
            }
        }
    }
}

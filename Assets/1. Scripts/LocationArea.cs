using System;
using System.Collections.Generic;
using UnityEngine;

public class LocationArea : MonoBehaviour
{
    public TerrainManager.LocationTag location;

    private HashSet<Agent> agents = new();

    private HashSet<BaseObject> objects = new();

    [SerializeField]
    private Transform position;

    private void Awake()
    {
        TerrainManager.AddLocation(
            location,
            new TerrainManager.LocationInfo { transform = position, area = this }
        );
    }

    public void Enter(PositionableObject obj)
    {
        //Debug.Log($"{obj.name}");
        if (obj is Agent agent)
            agents.Add(agent);
        else if (obj is BaseObject bObj)
            objects.Add(bObj);
    }

    public void Exit(PositionableObject obj)
    {
        if (obj is Agent agent)
            agents.Remove(agent);
        else if (obj is BaseObject bObj)
            objects.Remove(bObj);
    }

    public HashSet<Agent> GetAgents()
    {
        return agents;
    }

    public HashSet<BaseObject> GetBaseObjects()
    {
        return objects;
    }

    public BaseObject GetBaseObject(BaseObject.ObjectTag tag)
    {
        foreach (var _obj in objects)
        {
            //Debug.Log($"{tag} : {_obj.objectTag}");
            if (_obj.objectTag == tag)
            {
                return _obj;
            }
        }
        return null;
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

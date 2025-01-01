using System;
using UnityEngine;

public class SpeakCommand : ICommand
{
    public event Action OnStart;
    public event Action OnEnd;

    private Agent m_agent;
    private string m_speakStr;

    public SpeakCommand(Agent agent, string str)
    {
        this.m_agent = agent;
        this.m_speakStr = str;
    }

    public void Execute()
    {
        OnStart?.Invoke();
        var locationTag = m_agent.Area;
        var curArea = TerrainManager.LocationToArea(locationTag);
        foreach (var _agent in curArea.GetAgents())
        {
            if (m_agent != _agent)
            {
                _agent.OnEventListener(m_speakStr);
            }
        }
        OnEnd?.Invoke();
    }
}

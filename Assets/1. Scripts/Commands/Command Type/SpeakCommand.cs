using System;
using UnityEngine;

public class SpeakCommand : ICommand
{
    public event Action OnStart;
    public event Action OnEnd;

    private Agent m_agent;

    private string m_speakStr;
    private float m_volume;

    public SpeakCommand(Agent from, string str, float volume)
    {
        this.m_agent = from;
        this.m_speakStr = str;
        this.m_volume = volume;
    }

    public void Execute()
    {
        OnStart?.Invoke();
        var locationTag = m_agent.Area;
        var curArea = TerrainManager.LocationToArea(locationTag);

        curArea.MsgMediator(m_agent, m_speakStr, m_volume);

        OnEnd?.Invoke();
    }
}

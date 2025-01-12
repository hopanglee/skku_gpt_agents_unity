using System;
using Unity.VisualScripting;
using UnityEngine;

public class InteractObjectCommand : ICommand
{
    public event Action OnStart;
    public event Action OnEnd;

    private Agent m_agent; // 행위자
    private BaseObject m_baseObject;
    private string m_content;

    public InteractObjectCommand(Agent agent, BaseObject baseObject, string content)
    {
        this.m_agent = agent;
        this.m_baseObject = baseObject;
        this.m_content = content;
    }

    public void Execute()
    {
        OnStart?.Invoke();
        m_baseObject.Interact(m_agent, m_content);
        OnEnd?.Invoke();
    }
}

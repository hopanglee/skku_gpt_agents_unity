using System;
using Unity.VisualScripting;
using UnityEngine;

public class InteractCommand : ICommand
{
    public event Action OnStart;
    public event Action OnEnd;

    private Agent m_agent; // 행위자
    private IInteractable m_target;
    private string m_content;

    public InteractCommand(Agent agent, IInteractable target, string content)
    {
        this.m_agent = agent;
        this.m_target = target;
        this.m_content = content;
    }

    public void Execute()
    {
        OnStart?.Invoke();
        m_target.Interact(m_agent, m_content);
        OnEnd?.Invoke();
    }
}

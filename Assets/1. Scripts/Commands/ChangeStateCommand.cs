using System;
using Unity.VisualScripting;
using UnityEngine;

public class ChangeStateCommand : ICommand
{
    public event Action OnStart;
    public event Action OnEnd;

    private Agent m_agent;
    private string new_state;

    public ChangeStateCommand(Agent agent, string state)
    {
        this.m_agent = agent;
        this.new_state = state;
    }

    public void Execute()
    {
        OnStart?.Invoke();
        m_agent.SetState(new_state);
        OnEnd?.Invoke();
    }
}

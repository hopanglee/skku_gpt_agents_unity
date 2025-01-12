using System;
using UnityEngine;
using UnityEngine.TerrainUtils;

public class MoveCommand : ICommand
{
    public event Action OnStart;
    public event Action OnEnd;
    public bool isExecuting = false;

    private MoveController m_moveController;
    private Transform m_target;

    public MoveCommand(
        Agent agent,
        Transform target,
        Action startAction = null,
        Action endAction = null
    )
    {
        this.m_moveController = agent.GetComponent<MoveController>();
        this.m_target = target;

        OnStart += startAction;
        OnEnd += endAction;
    }

    public void Execute()
    {
        isExecuting = true;
        OnStart?.Invoke();

        m_moveController.Reset();
        m_moveController.OnReached += WhenReached;
        m_moveController.SetTarget(m_target);
    }

    public void WhenReached()
    {
        m_moveController.OnReached -= WhenReached;
        OnEnd?.Invoke();
        isExecuting = false;
    }
}

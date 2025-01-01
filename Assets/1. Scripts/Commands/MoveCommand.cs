using System;
using UnityEngine;
using UnityEngine.TerrainUtils;

public class MoveCommand : ICommand
{
    public event Action OnStart;
    public event Action OnEnd;
    public bool isExecuting = false;

    private MoveController m_moveController;
    private TerrainManager.LocationTag m_area;

    public MoveCommand(
        Agent agent,
        TerrainManager.LocationTag area,
        Action startAction = null,
        Action endAction = null
    )
    {
        this.m_moveController = agent.GetComponent<MoveController>();
        this.m_area = area;

        OnStart += startAction;
        OnEnd += endAction;
    }

    public void Execute()
    {
        isExecuting = true;
        OnStart?.Invoke();

        m_moveController.Reset();
        m_moveController.OnReached += WhenReached;
        m_moveController.SetTarget(TerrainManager.LocationToTransform(m_area));
    }

    public void WhenReached()
    {
        m_moveController.OnReached -= WhenReached;
        OnEnd?.Invoke();
        isExecuting = false;
    }
}

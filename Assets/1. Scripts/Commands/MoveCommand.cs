using System;
using UnityEngine;
using UnityEngine.TerrainUtils;

public class MoveCommand : ICommand
{
    public event Action OnStart;
    public event Action OnEnd;
    public bool isExecuting = false;

    private MoveController m_moveController;
    private TerrainManager.Location m_area;

    public MoveCommand(
        MoveController moveController,
        TerrainManager.Location area,
        Action startAction = null,
        Action endAction = null
    )
    {
        this.m_moveController = moveController;
        this.m_area = area;

        OnStart += startAction;
        OnEnd += endAction;
    }

    public void Execute()
    {
        isExecuting = true;
        OnStart?.Invoke();
        m_moveController.OnReached += WhenReached;
        m_moveController.SetTarget(TerrainManager.GetLocationTransform(m_area));
    }

    public void WhenReached()
    {
        m_moveController.OnReached -= WhenReached;
        OnEnd?.Invoke();
        isExecuting = false;
    }
}

using System;
using UnityEngine;
using UnityEngine.TerrainUtils;

public class MoveCommand : ICommand
{
    public event Action OnStart;
    public event Action OnEnd;

    private MoveController m_moveController;
    private TerrainManager.Location m_area;

    public MoveCommand(MoveController moveController, TerrainManager.Location area)
    {
        this.m_moveController = moveController;
        this.m_area = area;
    }

    public void Execute()
    {
        OnStart?.Invoke();

        m_moveController.SetTarget(TerrainManager.Instance.GetLocationVector(m_area));

        OnEnd?.Invoke();
    }
}

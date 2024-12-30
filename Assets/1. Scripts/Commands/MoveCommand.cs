using System;
using UnityEngine;

public class MoveCommand:ICommand
{
    public event Action OnStart;
    public event Action OnEnd;

    private MoveController m_moveController;

    public MoveCommand(MoveController moveController)
    {
        this.m_moveController = moveController;
    }

    public void Execute()
    {
        OnStart?.Invoke();

        OnEnd?.Invoke();
    }
}

using System;
using UnityEngine;

public interface ICommand
{
    public event Action OnStart;
    public event Action OnEnd;

    public void Execute();
}

using System;
using UnityEngine;

public class SpeakCommand : ICommand
{
    public event Action OnStart;
    public event Action OnEnd;

    public SpeakCommand(string str) { }

    public void Execute()
    {
        OnStart?.Invoke();

        OnEnd?.Invoke();
    }
}

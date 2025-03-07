using System;
using System.Collections.Generic;
using UnityEngine;

public class CommandInvoker
{
    private Queue<ICommand> m_commandQueue = new();

    public void EnqueueCommand(ICommand command)
    {
        m_commandQueue.Enqueue(command);
    }

    public void ExcuteNextCommand()
    {
        if (m_commandQueue.Count > 0)
        {
            ICommand command = m_commandQueue.Dequeue();
            Debug.Log($"{command.GetType()} Excuted.");
            command.Execute();
        }
    }

    public void ExcuteCommandDirectly(ICommand command)
    {
        command.Execute();
    }

    public void Clear()
    {
        m_commandQueue.Clear();
    }
}

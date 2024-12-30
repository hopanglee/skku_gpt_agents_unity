using System;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager
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
            command.Execute();
        }
    }

    public void ExcuteCommandDirectly(ICommand command)
    {
        command.Execute();
    }
}

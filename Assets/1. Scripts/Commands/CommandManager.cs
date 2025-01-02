using System;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager
{
    private Agent m_agent;
    private CommandInvoker m_commandInvoker;

    public CommandManager(Agent agent)
    {
        this.m_agent = agent;
        m_commandInvoker = new CommandInvoker();
    }

    public void AddMoveCommand(TerrainManager.LocationTag location)
    {
        var command = CreateMoveCommand(location);
        m_commandInvoker.EnqueueCommand(command);
    }

    public void AddSpeakCommand(string str)
    {
        var command = CreateSpeakCommand(str);
        m_commandInvoker.EnqueueCommand(command);
    }

    public void AddStateCommand(string str)
    {
        var command = CreateStateCommand(str);
        m_commandInvoker.EnqueueCommand(command);
    }

    private MoveCommand CreateMoveCommand(TerrainManager.LocationTag location)
    {
        var command = new MoveCommand(m_agent, location);
        command.OnEnd += m_commandInvoker.ExcuteNextCommand;

        return command;
    }

    private SpeakCommand CreateSpeakCommand(string str)
    {
        var command = new SpeakCommand(m_agent, str);
        command.OnEnd += m_commandInvoker.ExcuteNextCommand;
        return command;
    }

    private ChangeStateCommand CreateStateCommand(string str)
    {
        var command = new ChangeStateCommand(m_agent, str);
        command.OnEnd += m_commandInvoker.ExcuteNextCommand;
        return command;
    }

    public void Execute()
    {
        m_commandInvoker.ExcuteNextCommand();
    }
}

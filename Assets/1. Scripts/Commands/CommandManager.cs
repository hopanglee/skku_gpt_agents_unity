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

    public void AddMoveCommand(Transform target)
    {
        var command = CreateMoveCommand(target);
        m_commandInvoker.EnqueueCommand(command);
    }

    public void AddMoveToLocationCommand(TerrainManager.LocationTag location)
    {
        var locationTr = TerrainManager.LocationToTransform(location);
        var command = CreateMoveCommand(locationTr);
        m_commandInvoker.EnqueueCommand(command);
    }

    public void AddChaseCommand(AgentName agentName)
    {
        var agentTr = AgentManager.GetAgentByName(agentName).transform;
        var command = CreateMoveCommand(agentTr);
        m_commandInvoker.EnqueueCommand(command);
    }

    public void AddSpeakCommand(string str, float volume)
    {
        var command = CreateSpeakCommand(str, volume);
        m_commandInvoker.EnqueueCommand(command);
    }

    public void AddStateCommand(string str)
    {
        var command = CreateStateCommand(str);
        m_commandInvoker.EnqueueCommand(command);
    }

    private MoveCommand CreateMoveCommand(Transform target)
    {
        var command = new MoveCommand(m_agent, target);
        command.OnEnd += m_commandInvoker.ExcuteNextCommand;

        return command;
    }

    private SpeakCommand CreateSpeakCommand(string str, float volume)
    {
        var command = new SpeakCommand(m_agent, str, volume);
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

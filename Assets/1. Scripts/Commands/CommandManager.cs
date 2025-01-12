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

    public void ResponseToCommand(AINPCResponse response)
    {
        switch (response.Command)
        {
            case AINPCResponse.CommandType.ChangeState:
                AddStateCommand(response.Parameter[0].ToString());

                break;

            case AINPCResponse.CommandType.Chase:
                var agentName = StringToAgentName(response.Parameter[0].ToString());
                AddChaseCommand(agentName);
                break;

            case AINPCResponse.CommandType.Interact:
                var objectTag = StringToObjectTag(response.Parameter[0].ToString());
                AddInteractToObjectCommand(objectTag, response.Parameter[1].ToString());
                break;

            case AINPCResponse.CommandType.MoveToLocation:
                var locationTag = StringToLocationTag(response.Parameter[0].ToString());
                AddMoveToLocationCommand(locationTag);
                break;

            case AINPCResponse.CommandType.MoveToObject:
                var _objectTag = StringToObjectTag(response.Parameter[0].ToString());
                AddMoveToObjectCommand(_objectTag);
                
                break;

            case AINPCResponse.CommandType.Speak:
                AddSpeakCommand(response.Parameter[0].ToString(), (float)response.Parameter[1]);
                break;
        }
    }

    private AgentName StringToAgentName(string str)
    {
        if (Enum.TryParse(str, true, out AgentName agentName))
        {
            return agentName;
        }
        else return AgentName.None;
    }

    private BaseObject.ObjectTag StringToObjectTag(string str)
    {
        if (Enum.TryParse(str, true, out BaseObject.ObjectTag objectTag))
        {
            return objectTag;
        }
        else return BaseObject.ObjectTag.None;
    }

    private TerrainManager.LocationTag StringToLocationTag(string str)
    {
        if (Enum.TryParse(str, true, out TerrainManager.LocationTag locationTag))
        {
            return locationTag;
        }
        else return TerrainManager.LocationTag.None;

    }

    #region Add Command
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

    public void AddMoveToObjectCommand(BaseObject.ObjectTag objectTag)
    {
        //var locationTr = TerrainManager.LocationToTransform(null);
        var objTr = TerrainManager
            .LocationToArea(m_agent.Area)
            ?.GetBaseObject(objectTag)
            ?.GetTransform();
        //Debug.Log($"{objTr.position}");
        if (objTr == null)
        {
            return;
        }

        var command = CreateMoveCommand(objTr);
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

    public void AddInteractToObjectCommand(BaseObject.ObjectTag objectTag, string content)
    {
        var obj = TerrainManager
            .LocationToArea(m_agent.Area)
            ?.GetBaseObject(objectTag);
        if (obj == null)
        {
            return;
        }
        var command = CreateInteractToObjectCommand(obj, content);
        m_commandInvoker.EnqueueCommand(command);
    }
    #endregion

    #region Create Command

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

    private InteractObjectCommand CreateInteractToObjectCommand(BaseObject tag, string str)
    {
        var command = new InteractObjectCommand(m_agent, tag, str);
        command.OnEnd += m_commandInvoker.ExcuteNextCommand;
        return command;
    }

    #endregion

    public void Execute()
    {
        m_commandInvoker.ExcuteNextCommand();
    }
}

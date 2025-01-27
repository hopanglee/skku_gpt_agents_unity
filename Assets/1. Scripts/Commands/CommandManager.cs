using System;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using Newtonsoft.Json;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
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

    public void Reset()
    {
        m_commandInvoker.Clear();
        m_agent.GetComponent<MoveController>().Reset();
    }

    // 나중에 PostForRawJson이걸로 가져옴
    public void AddCommandFromResponseJson(AINPCResponse response)
    {
        //var response = JsonConvert.DeserializeObject<AINPCResponse>(responseJson);
        switch (response.Command)
        {
            case AINPCResponse.CommandType.ChangeState:
                var changeStateResponse = JsonConvert.DeserializeObject<ChangeStateResponse>(
                    JsonConvert.SerializeObject(response)
                );
                AddStateCommand(changeStateResponse.NewState);

                break;

            case AINPCResponse.CommandType.Chase:
                var chaseResponse = JsonConvert.DeserializeObject<ChaseResponse>(
                    JsonConvert.SerializeObject(response)
                );
                AddChaseCommand(chaseResponse.Target);
                break;

            case AINPCResponse.CommandType.Interact:
                var interactResponse = JsonConvert.DeserializeObject<InteractResponse>(
                    JsonConvert.SerializeObject(response)
                );
                AddInteractToObjectCommand(interactResponse.ObjectTag, interactResponse.content);
                break;

            case AINPCResponse.CommandType.MoveToLocation:
                var moveToLocationResponse = JsonConvert.DeserializeObject<MoveToLocationResponse>(
                    JsonConvert.SerializeObject(response)
                );
                AddMoveToLocationCommand(moveToLocationResponse.LocationTag);
                break;

            case AINPCResponse.CommandType.MoveToObject:
                var moveToObjectResponse = JsonConvert.DeserializeObject<MoveToObjectResponse>(
                    JsonConvert.SerializeObject(response)
                );
                AddMoveToObjectCommand(moveToObjectResponse.ObjectTag);

                break;

            case AINPCResponse.CommandType.Speak:
                var speakResponse = JsonConvert.DeserializeObject<SpeakResponse>(
                    JsonConvert.SerializeObject(response)
                );

                AddSpeakCommand(speakResponse.Content, speakResponse.Volume);
                break;
        }
    }

    public static AgentName StringToAgentName(string str)
    {
        if (Enum.TryParse(str, true, out AgentName agentName))
        {
            return agentName;
        }
        else
            return AgentName.None;
    }

    public static BaseObject.ObjectTag StringToObjectTag(string str)
    {
        if (Enum.TryParse(str, true, out BaseObject.ObjectTag objectTag))
        {
            return objectTag;
        }
        else
            return BaseObject.ObjectTag.None;
    }

    public static TerrainManager.LocationTag StringToLocationTag(string str)
    {
        if (Enum.TryParse(str, true, out TerrainManager.LocationTag locationTag))
        {
            return locationTag;
        }
        else
            return TerrainManager.LocationTag.None;
    }

    #region Add Command
    public void AddCommand(ICommand command)
    {
        m_commandInvoker.EnqueueCommand(command);
    }

    #region AI NPC
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

    public void AddInteractToObjectCommand(string objectTag, string content)
    {
        IInteractable obj = null;

        var baseObjectTag = StringToObjectTag(objectTag);
        //var agentName = StringToAgentName(objectTag);

        if (baseObjectTag != BaseObject.ObjectTag.None)
        {
            obj = TerrainManager.LocationToArea(m_agent.Area)?.GetBaseObject(baseObjectTag); // 변환 성공 시 사용
        }
        // else if (agentName != AgentName.None)
        // {
        //     obj = AgentManager.GetAgentByName(agentName);
        // }

        // 적합한 객체를 찾지 못하면 로그 출력
        if (obj == null)
        {
            Debug.LogWarning($"No interactable object found for tag: {objectTag}");
            return;
        }

        // InteractCommand 생성 및 큐에 추가
        var command = CreateInteractCommand(obj, content);
        m_commandInvoker.EnqueueCommand(command);
    }

    #endregion

    #region BaseObject


    #endregion

    #endregion

    #region Create Command
    #region AI NPC
    private MoveCommand CreateMoveCommand(Transform target)
    {
        var command = new MoveCommand(m_agent, target);
        command.OnEnd += m_commandInvoker.ExcuteNextCommand;
        command.OnEnd += m_agent.CommandIsComplete;

        return command;
    }

    private SpeakCommand CreateSpeakCommand(string str, float volume)
    {
        var command = new SpeakCommand(m_agent, str, volume);
        command.OnEnd += m_commandInvoker.ExcuteNextCommand;
        command.OnEnd += m_agent.CommandIsComplete;
        return command;
    }

    private ChangeStateCommand CreateStateCommand(string str)
    {
        var command = new ChangeStateCommand(m_agent, str);
        command.OnEnd += m_commandInvoker.ExcuteNextCommand;
        command.OnEnd += m_agent.CommandIsComplete;
        return command;
    }

    private InteractCommand CreateInteractCommand(IInteractable tag, string str)
    {
        var command = new InteractCommand(m_agent, tag, str);
        command.OnEnd += m_commandInvoker.ExcuteNextCommand;
        command.OnEnd += m_agent.CommandIsComplete;
        return command;
    }

    #endregion

    #region  BaseObject

    #endregion
    #endregion

    public void Execute()
    {
        m_commandInvoker.ExcuteNextCommand();
    }
}

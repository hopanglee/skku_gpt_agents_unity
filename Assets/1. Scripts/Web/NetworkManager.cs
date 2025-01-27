using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Pathfinding;
using UnityEngine;

#region  Response
public class AINPCResponses
{
    [JsonProperty("commands")]
    public AINPCResponse[] commands;
    [JsonProperty("message")]
    public string message;
}

public class AINPCResponse
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CommandType
    {
        MoveToLocation,
        MoveToObject,
        Chase,
        Speak,
        ChangeState,
        Interact,
    }

    [JsonProperty("command_type")] // JSON 필드 이름
    public CommandType Command { get; set; }
    [JsonProperty("message")]
    public string message;
}

#region Command Json Class
public class MoveToLocationResponse : AINPCResponse
{
    [JsonProperty("location")]
    public TerrainManager.LocationTag LocationTag { get; set; }
}

public class MoveToObjectResponse : AINPCResponse
{
    [JsonProperty("object")]
    public BaseObject.ObjectTag ObjectTag { get; set; }
}

public class ChaseResponse : AINPCResponse
{
    [JsonProperty("target_npc")]
    public AgentName Target { get; set; }
}

public class SpeakResponse : AINPCResponse
{
    [JsonProperty("content")]
    public string Content { get; set; }

    [JsonProperty("volume")]
    public int Volume { get; set; }
}

public class ChangeStateResponse : AINPCResponse
{
    [JsonProperty("new_state")]
    public string NewState { get; set; }
}

public class InteractResponse : AINPCResponse
{
    [JsonProperty("target_object")]
    public string ObjectTag { get; set; }

    [JsonProperty("interact_content")]
    public string content { get; set; }
}

#endregion

public class BaseObjectResponse
{
    [JsonProperty("new_front_state")]
    public string newFrontState;

    [JsonProperty("new_back_state")]
    public string newBackState;

    public class InteractContent
    {
        [JsonProperty("target_object")]
        public string objectTag;

        [JsonProperty("interact_content")]
        public string content;
    }

    [JsonProperty("interact_content")]
    public InteractContent interactContent;

    [JsonProperty("message")]
    public string message;
}

#endregion


#region Request

public class SerializableVector3
{
    [JsonProperty("x")]
    public float x;

    [JsonProperty("y")]
    public float y;

    [JsonProperty("z")]
    public float z;

    public SerializableVector3(Vector3 vector)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
}
public class AINPCRequest
{
    public class AgentInformation
    {
        [JsonProperty("agent_name")]
        public AgentName agentName;

        [JsonProperty("position")]
        public SerializableVector3 position;

        [JsonProperty("state")]
        public string state;
    }

    public class BaseObjectInformation
    {
        [JsonProperty("object_tag")]
        public BaseObject.ObjectTag objectTag;

        [JsonProperty("position")]
        public SerializableVector3 position;

        [JsonProperty("state")]
        public string state;
    }

    public class TalkInfo
    {
        [JsonProperty("source")]
        public AgentName agentName;

        [JsonProperty("content")]
        public string content;
    }

    [JsonProperty("my_info")]
    public AgentInformation myInformation;

    [JsonProperty("area_tag")]
    public TerrainManager.LocationTag areaName;

    [JsonProperty("other_agent_infos")]
    public AgentInformation[] otherAgents;

    [JsonProperty("other_baseobject_infos")]
    public BaseObjectInformation[] otherBaseObjects;

    [JsonProperty("talk_infos")]
    public TalkInfo[] talkInfos;
}

public class BaseObjectRequest
{
    [JsonProperty("object_tag")]
    public BaseObject.ObjectTag objectTag;

    [JsonProperty("id")]
    public int id;

    [JsonProperty("npc_name")]
    public AgentName agentName;

    [JsonProperty("interact_content")]
    public string interactContent;
}

#endregion


public static class NetworkManager
{
    static readonly HttpWebFetcher fetcher = new("http://localhost:8000/api");

    public static string BaseUri
    {
        get { return fetcher.baseUri; }
        set { fetcher.baseUri = value; }
    }

    public static async Task<AINPCResponses> AINPCPost(AINPCRequest request)
    {
        try
        {
            return await fetcher.Post<AINPCResponses>("/agent", request);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }

        return null;
    }

    public static async Task<AINPCResponse> AINPCStartPost(AINPCRequest request)
    {
        try
        {
            return await fetcher.Post<AINPCResponse>("/agent/start", request);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }

        return null;
    }

    public static async Task<AINPCResponse> AINPCCommandCompletedPost(AINPCRequest request)
    {
        try
        {
            return await fetcher.Post<AINPCResponse>("/agent/completed", request);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }

        return null;
    }

    public static async Task<BaseObjectResponse> BaseObjectPost(BaseObjectRequest request)
    {
        try
        {
            return await fetcher.Post<BaseObjectResponse>("/base_object", request);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }

        return null;
    }

    public static async Task<BaseObjectResponse> BaseObjectStartPost(BaseObjectRequest request)
    {
        try
        {
            return await fetcher.Post<BaseObjectResponse>("/base_object/start", request);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }

        return null;
    }

    public static async Task<string> PostForRawJson(AINPCRequest request)
    {
        try
        {
            return await fetcher.Post<string>("/chat", request);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }

        return null;
    }
}

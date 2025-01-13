using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Pathfinding;
using UnityEngine;

#region  Response 
public class AINPCResponse
{
    public enum CommandType { MoveToLocation, MoveToObject, Chase, Speak, ChangeState, Interact }
    [JsonProperty("command_type")] // JSON 필드 이름
    public CommandType Command { get; set; }
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
    public float Volume { get; set; }
}

public class ChangeStateResponse : AINPCResponse
{
    [JsonProperty("new_state")]
    public string NewState { get; set; }
}

public class InteractResponse : AINPCResponse
{
    [JsonProperty("target_object")]
    public BaseObject.ObjectTag ObjectTag { get; set; }

    [JsonProperty("interact_content")]
    public string content { get; set; }
}

#endregion

public class BaseObjectResponse
{
    [JsonProperty("new_state")]
    public string newState;
}

#endregion

#region Request
public class AINPCRequest
{
    [JsonProperty("npc_name")]
    public AgentName agentName;

    [JsonProperty("interact_contents")]
    public string[] interactContents;

    [JsonProperty("information")] // 단순 사실 정보 ex. 누구랑은 몇m 떨어져 있다. 등
    public string[] informations;
}

public class BaseObjectRequest
{
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


    public static async Task<AINPCResponse> AINPCPost(AINPCRequest request)
    {
        try
        {
            return await fetcher.Post<AINPCResponse>("/chat", request);
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
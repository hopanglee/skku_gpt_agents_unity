using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;


public class AINPCResponse
{
    public enum CommandType{MoveToLocation, MoveToObject, Chase, Speak, ChangeState, Interact}
    [JsonProperty("command_type")] // JSON 필드 이름
    public CommandType Command { get; set; }
    
    [JsonProperty("parameter")] // 기타 데이터 필드
    public object[] Parameter { get; set; }

}

public class AINPCRequest
{

}

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
}
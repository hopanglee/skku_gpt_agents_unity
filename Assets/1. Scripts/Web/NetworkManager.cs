using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;


public class Response
{

}

public class Request
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


    public static async Task<Response> Post(Request request)
    {
        try
        {
            return await fetcher.Post<Response>("/chat", request);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }

        return null;
    }
}
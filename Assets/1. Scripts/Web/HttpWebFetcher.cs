using System;
using System.Collections;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class HttpWebFetcher
{
    public string baseUri;

    public HttpWebFetcher(string baseUri)
    {
        this.baseUri = baseUri;
    }

    private UnityWebRequest CreateRequest(string url, string method, object data)
    {
        UnityWebRequest request = new(baseUri + url, method, new DownloadHandlerBuffer(), null);

        if (data is not null)
        {
            Debug.Log(JsonConvert.SerializeObject(data));
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        }
        request.SetRequestHeader("Content-Type", "application/json");

        return request;
    }

    private async Task<T> SendRequest<T>(string url, string method, object data)
    {
        using UnityWebRequest request = CreateRequest(url, method, data);

        var operation = request.SendWebRequest();
        while (!operation.isDone)
        {
            await Task.Yield();
        }

        switch (request.result)
        {
            case UnityWebRequest.Result.Success:
                string responseJson = request.downloadHandler.text;
                Debug.Log("Get Json From Server > " + responseJson);
                try
                {
                    var responseData = JsonConvert.DeserializeObject<T>(responseJson);
                    return responseData;
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                    throw new Exception(e.Message);
                }
            case UnityWebRequest.Result.InProgress:
                Debug.LogError("In Progress, Not Waiting Above");
                throw new Exception("In Progress, Not Waiting Above");
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.ProtocolError:
            case UnityWebRequest.Result.DataProcessingError:
                Debug.LogError(request.error);
                throw new Exception(request.error);
            default:
                throw new Exception("Unknown error");
        }
    }

    public async Task<T> Get<T>(string url)
    {
        try
        {
            return await SendRequest<T>(url, "GET", null);
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<T> Post<T>(string url, object data)
    {
        try
        {
            return await SendRequest<T>(url, "POST", data);
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<T> Put<T>(string url, object data)
    {
        try
        {
            return await SendRequest<T>(url, "PUT", data);
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<T> Delete<T>(string url)
    {
        try
        {
            return await SendRequest<T>(url, "DELETE", null);
        }
        catch (Exception e)
        {
            throw e;
        }
    }
}

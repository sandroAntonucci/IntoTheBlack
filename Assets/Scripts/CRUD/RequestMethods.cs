using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using System.Text.Json;
using Unity.VisualScripting.Antlr3.Runtime;
using Unity.VisualScripting;


public class RequestMethods
{
    private static void SetHeaders(UnityWebRequest request, string token)
    {
        request.SetRequestHeader("Content-Type", "application/json");
        if (!string.IsNullOrEmpty(token))
        {
            request.SetRequestHeader("Authorization", $"Bearer {token}");
        }
    }

    private static void HandleResponse<T>(UnityWebRequest request, ref Action<T> onSuccess, ref Action<string> onError) where T : class
    {
        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            T response = JsonUtility.FromJson<T>(jsonResponse);
            onSuccess?.Invoke(response);
        }
        else
        {
            string errorMessage = !string.IsNullOrEmpty(request.downloadHandler.text) ? 
                                                        request.downloadHandler.text : 
                                                        request.error;
            onError?.Invoke(errorMessage);
        }
    }

    public static IEnumerator PostRequest<T>(string url, string body, Action<T> onSuccess, Action<string> onError, string token = null) where T : class
    {
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(body);

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            SetHeaders(request, token);

            yield return request.SendWebRequest();

            HandleResponse(request, ref onSuccess, ref onError);
        }
    }

    public static IEnumerator PostRequest<T>(string url, Action<T> onSuccess, Action<string> onError, string token = null) where T : class
    { 
        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes("");

            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            SetHeaders(request, token);

            yield return request.SendWebRequest();

            HandleResponse(request, ref onSuccess, ref onError);
        }
    }

    public static IEnumerator GetRequest<T>(string url, Action<T> onSuccess, Action<string> onError, string token) where T : class
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            SetHeaders(request, token);

            yield return request.SendWebRequest();

            HandleResponse(request, ref onSuccess, ref onError);
        }
    }

    public static IEnumerator PutRequest<T>(string url, Action<T> onSuccess, Action<string> onError, string token) where T : class
    {
        using (UnityWebRequest request = new UnityWebRequest(url, "PUT"))
        {
            request.downloadHandler = new DownloadHandlerBuffer();
            SetHeaders(request, token);

            yield return request.SendWebRequest();

            HandleResponse(request, ref onSuccess, ref onError);
        }
    }


    public static IEnumerator DeleteRequest<T>(string url, Action<T> onSuccess, Action<string> onError, string token) where T : class
    {
        using (UnityWebRequest request = UnityWebRequest.Delete(url))
        {
            request.downloadHandler = new DownloadHandlerBuffer();
            SetHeaders(request, token);

            yield return request.SendWebRequest();

            HandleResponse(request, ref onSuccess, ref onError);
        }
    }
}
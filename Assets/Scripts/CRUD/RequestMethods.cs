using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using System.Text.Json;


public static class RequestMethods
{
    public static IEnumerator PostRequest<T>(string url, string body, Action<T> onSuccess, Action<string> onError) where T : class
    {
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(body);

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                T response = JsonUtility.FromJson<T>(jsonResponse);
                onSuccess?.Invoke(response);
            }
            else
            {
                onError?.Invoke(request.downloadHandler.text);
            }
        }
    }
}


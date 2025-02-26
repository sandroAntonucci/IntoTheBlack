using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using System.Text.Json;
using UnityEditor.PackageManager.Requests;

public class PlayerCRUD
{
    public static IEnumerator CreatePlayer(UserData user, Action onSuccess, Action<string> onError)
    {
        string url = Endpoints.ApiUrlCloud + Endpoints.CreatePlayer;
        string jsonData = JsonUtility.ToJson(user);

        yield return RequestMethods.PostRequest<Player>(url, jsonData, response =>
        {
            Debug.Log(response);
            onSuccess?.Invoke();
        }, error =>
        {
            Debug.Log(error);
            onError?.Invoke(error);
        }, GameManager.Instance.AuthUser.token);
    }

}


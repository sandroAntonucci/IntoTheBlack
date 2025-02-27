using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using System.Text.Json;
using UnityEditor.PackageManager.Requests;

public class UserCRUD
{
    public static IEnumerator SelectAllUserPlayers(Action<PlayerList> onSuccess, Action<string> onError)
    {
        string username = GameManager.Instance.AuthUser.user.username;
        string url = string.Format($"{Endpoints.ApiUrlCloud}{Endpoints.GetUserPlayers}", username);

        yield return RequestMethods.GetRequest<PlayerList>(url, response =>
        {
            Debug.Log("All playeres are SELECTED");
            onSuccess?.Invoke(response);
        }, error =>
        {
            onError?.Invoke(error);
        }, GameManager.Instance.AuthUser.token);
    }
}


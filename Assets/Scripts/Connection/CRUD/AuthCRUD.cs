using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using System.Text.Json;
using UnityEditor.PackageManager.Requests;

public class AuthCRUD
{
    private static IEnumerator AuthRequest<T>(string endpoint, T data, Action onSuccess, Action<string> onError) where T : class
    {
        string url = Endpoints.ApiUrlCloud + endpoint;
        string jsonData = JsonUtility.ToJson(data);

        yield return RequestMethods.PostRequest<User>(url, jsonData, response =>
        {
            GameManager.Instance.AuthUser = response;
            onSuccess?.Invoke();

            Debug.Log($"Username: {GameManager.Instance.AuthUser.user.username}");
            Debug.Log($"Token: {GameManager.Instance.AuthUser.token}");
        }, error =>
        {
            GameManager.Instance.AuthUser = null;
            onError?.Invoke(error);
        });
    }

    public static IEnumerator Login(LoginRequest data, Action onSuccess, Action<string> onError)
    {
        yield return AuthRequest(Endpoints.Login, data, onSuccess, onError);
    }

    public static IEnumerator Register(RegisterRequest data, Action onSuccess, Action<string> onError)
    {
        yield return AuthRequest(Endpoints.Register, data, onSuccess, onError);
    }

}
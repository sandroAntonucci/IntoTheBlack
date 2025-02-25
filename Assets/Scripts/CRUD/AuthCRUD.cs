using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using System.Text.Json;
using UnityEditor.PackageManager.Requests;

public class AuthCRUD
{
    public IEnumerator Login(LoginRequest data, Action onSuccess, Action<string> onError)
    {
        string url = Endpoints.ApiUrlCloud + Endpoints.Login;
        string jsonData = JsonUtility.ToJson(data);

        yield return RequestMethods.PostRequest<User>(url, jsonData, (response) =>
        {
            GameManager.Instance.AuthUser = response;
            onSuccess?.Invoke();

            Debug.Log("Username: " + GameManager.Instance.AuthUser.user.username);
            Debug.Log("Token: " + GameManager.Instance.AuthUser.token);
        }, (error) =>
        {
            GameManager.Instance.AuthUser = null;
            onError?.Invoke(error);
        });
    }

    public IEnumerator Register(RegisterRequest data, Action onSuccess, Action<string> onError)
    {
        string url = Endpoints.ApiUrlCloud + Endpoints.Register;
        string jsonData = JsonUtility.ToJson(data);

        yield return RequestMethods.PostRequest<User>(url, jsonData, (response) =>
        {
            GameManager.Instance.AuthUser = response;
            onSuccess?.Invoke();

            Debug.Log("Username: " + GameManager.Instance.AuthUser.user.username);
            Debug.Log("Token: " + GameManager.Instance.AuthUser.token);
        }, (error) =>
        {
            GameManager.Instance.AuthUser = null;
            onError?.Invoke(error);
        });
    }
}
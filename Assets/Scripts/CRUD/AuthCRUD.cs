using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using System.Text.Json;

public class AuthCRUD
{
    public IEnumerator Login(UserRequestDTO data)
    {
        string url = Endpoints.ApiUrlCloud + Endpoints.Login;
        //string jsonData = "{" +
        //    $"\"username\": \"{data.Username}\", " +
        //    $"\"password\": \"{data.Password}\" " +
        //    "}";
        string jsonData = JsonUtility.ToJson(data);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Enviar la solicitud y esperar la respuesta
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            try
            {
                string jsonResponse = request.downloadHandler.text;
                User response = JsonSerializer.Deserialize<User>(jsonResponse);

                Debug.Log(request.downloadHandler.text);
                GameManager.Instance.User = response;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error al deserializar la respuesta: " + ex.Message);
                GameManager.Instance.User = null;
            }
        }
        else
        {
            Debug.LogError("Error en la solicitud: " + request.error);
            GameManager.Instance.User = null;
        }
    }

    public IEnumerator Register(UserRequestDTO data)
    {
        string url = Endpoints.ApiUrlCloud + Endpoints.Register;
        string jsonData = JsonUtility.ToJson(data);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Enviar la solicitud y esperar la respuesta
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            try
            {
                User response = JsonUtility.FromJson<User>(request.downloadHandler.text);
                GameManager.Instance.User = response;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error al deserializar la respuesta: " + ex.Message);
                GameManager.Instance.User = null;
            }
        }
        else
        {
            Debug.LogError("Error en la solicitud: " + request.error);
            GameManager.Instance.User = null;
        }
    }
}
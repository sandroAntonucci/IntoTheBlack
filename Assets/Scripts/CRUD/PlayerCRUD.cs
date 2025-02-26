using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using System.Text.Json;
using UnityEditor.PackageManager.Requests;

public class PlayerCRUD
{
    public static IEnumerator AddFragmentToPlayer(string fragmentId, Action onSuccess, Action<string> onError) 
    {
        string playerId = GameManager.Instance.CurrentPlayer.id.ToString();
        string url = string.Format($"{Endpoints.ApiUrlCloud}{Endpoints.SaveFragmentOnPlayer}", playerId, fragmentId);

        yield return RequestMethods.PostRequest<Player>(url, response =>
        {
            Debug.Log("Fragment ADDED: " + fragmentId);
            GameManager.Instance.CurrentPlayer = response;
            onSuccess?.Invoke();
        }, error =>
        {
            onError?.Invoke(error);
        }, GameManager.Instance.AuthUser.token);
    }

    public static IEnumerator UpdateRecordTime(Action onSuccess, Action<string> onError)
    {
        Player player = GameManager.Instance.CurrentPlayer;
        float recordTimeInSeconds = GameManager.Instance.timer;
        player.recordTime = GameManager.FloatToHMS(recordTimeInSeconds);

        string url = string.Format($"{Endpoints.ApiUrlCloud}{Endpoints.PlayerTime}", player.id);
        string finalUrl = $"{url}?recordTime={player.recordTime}";

        yield return RequestMethods.PutRequest<Player>(finalUrl, response =>
        {
            Debug.Log("Record Time UPDATED: " + response.recordTime);
            GameManager.Instance.CurrentPlayer = response;
            onSuccess?.Invoke();
        }, error =>
        {
            onError?.Invoke(error);
        }, GameManager.Instance.AuthUser.token);
    }

    public static IEnumerator CreatePlayer(UserData user, Action onSuccess, Action<string> onError)
    {
        string url = Endpoints.ApiUrlCloud + Endpoints.Players;
        string finalUrl = $"{url}?username={user.username}";

        yield return RequestMethods.PostRequest<Player>(finalUrl, response =>
        {
            Debug.Log("Player CREATED");
            GameManager.Instance.CurrentPlayer = response;
            onSuccess?.Invoke();
        }, error =>
        {
            GameManager.Instance.CurrentPlayer = null;
            onError?.Invoke(error);
        }, GameManager.Instance.AuthUser.token);
    }

    public static IEnumerator SelectPlayer(int id, Action onSuccess, Action<string> onError)
    {
        string url = string.Format($"{Endpoints.ApiUrlCloud}{Endpoints.PlayersWithId}", id);
        
        yield return RequestMethods.GetRequest<Player>(url, response =>
        {
            Debug.Log("Player SELECTED: " + response.id);
            onSuccess?.Invoke();
        }, error =>
        {
            onError?.Invoke(error);
        }, GameManager.Instance.AuthUser.token);
    }

    public static IEnumerator DeletePlayer(int id, Action onSuccess, Action<string> onError)
    {
        string url = string.Format($"{Endpoints.ApiUrlCloud}{Endpoints.PlayersWithId}", id);

        yield return RequestMethods.DeleteRequest<string>(url, response =>
        {
            Debug.Log(response);
            onSuccess?.Invoke();
        }, error =>
        {
            onError?.Invoke(error);
        }, GameManager.Instance.AuthUser.token);
    }
}


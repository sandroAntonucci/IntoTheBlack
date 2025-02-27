using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Profiling;

public class ButtonForManagePlayers : MonoBehaviour
{
    public void RemovePlayer()
    {
        StartCoroutine(PlayerCRUD.DeletePlayer(
             GetPlayerId(),
             ResetManaegScene,
             error => Debug.LogError(error)
         ));
    }

    public void SelectPlayer()
    {
        StartCoroutine(PlayerCRUD.SelectPlayer(
            GetPlayerId(),
            StartGame,
            error => Debug.LogError(error)
        ));
    }

    public void CreatePlayer()
    {
        StartCoroutine(PlayerCRUD.CreatePlayer(
            GameManager.Instance.AuthUser.user,
            ResetManaegScene,
            error => Debug.LogError(error)
        ));
    }

    private int GetPlayerId()
    {
        GameObject parentObject = transform.parent.gameObject;
        GameObject grandParentObject = parentObject.transform.parent.gameObject;
        PlayerManagementScene managementScene = grandParentObject.GetComponent<PlayerManagementScene>();
        
        Player selectedPlayer = managementScene.ListOfPlayers.players[GetProfileId()];
        return selectedPlayer.id;
    }

    private int GetProfileId()
    {
        GameObject selectButton = transform.GetChild(0).gameObject;
        GameObject nameObject = selectButton.transform.GetChild(0).gameObject;
        TextMeshProUGUI nameText = nameObject.GetComponent<TextMeshProUGUI>();

        char lastChar = nameText.text[nameText.text.Length - 1];
        return int.Parse(lastChar.ToString()) - 1;
    }

    private void ResetManaegScene()
    {
        GameObject parentObject = transform.parent.gameObject;
        GameObject grandParentObject = parentObject.transform.parent.gameObject;
        PlayerManagementScene managementScene = grandParentObject.GetComponent<PlayerManagementScene>();
        StartCoroutine(managementScene.ReinitializeInterface());
    }

    private void StartGame()
    {
        GameManager.Instance.timer = 0;
        SceneManager.LoadScene("LevelOne");
    }
}

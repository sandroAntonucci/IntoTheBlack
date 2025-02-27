using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class PlayerManagementScene : MonoBehaviour
{
    public List<GameObject> profilesLocation;
    public GameObject profilePrefab;
    public GameObject addProfilePrefab;

    private PlayerList listOfPlayers;

    public PlayerList ListOfPlayers { get => listOfPlayers; set => listOfPlayers = value; }

    void OnEnable()
    {
        StartCoroutine(InitializeInterface());
    }

    private IEnumerator InitializeInterface()
    {
        yield return StartCoroutine(UserCRUD.SelectAllUserPlayers(response =>
        {
            listOfPlayers = response;
        }, error =>
        {
            Debug.LogError(error);
        }));


        for (int i = 0; i < profilesLocation.Count; i++)
        {
            if (listOfPlayers.players.Count > i)
            {
                GameObject profile = Instantiate(profilePrefab, profilesLocation[i].transform);
                GameObject selectButton = profile.transform.GetChild(0).gameObject;

                // Modificamos el numero del perfil
                GameObject nameObject = selectButton.transform.GetChild(0).gameObject;
                TextMeshProUGUI nameText = nameObject.GetComponent<TextMeshProUGUI>();
                nameText.text += $" {i + 1}";

                // Modificamos el valor del tiempo de finalización del jugador
                GameObject timeObject = selectButton.transform.GetChild(1).gameObject;
                TextMeshProUGUI timeText = timeObject.GetComponent<TextMeshProUGUI>();
                timeText.text = listOfPlayers.players[i].recordTime;
            }
            else
            {
                GameObject profile = Instantiate(addProfilePrefab, profilesLocation[i].transform);
            }
        }
    }

    private void RemoveChild(List<GameObject> objects)
    {
        foreach (GameObject obj in objects)
        {
            foreach (Transform child in obj.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }

    public IEnumerator ReinitializeInterface()
    {
        RemoveChild(profilesLocation);
        yield return StartCoroutine(InitializeInterface());
    }
}

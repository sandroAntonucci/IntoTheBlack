using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableCollectable : MonoBehaviour
{
    private bool isCollected = false;
    [SerializeField] private int id;

    [SerializeField] private MoveMenuCamera cameraZoomIn;
    [SerializeField] private MoveMenuCamera cameraZoomOut;

    private bool playerCanInteract;
    private bool playerIsInteracting;

    private bool cameraZoom;

    private string currentText = "[E] to Open";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerObj"))
        {
            PlayerInterface.Instance.TextToPlayer.text = currentText;
            playerCanInteract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerObj"))
        {
            PlayerInterface.Instance.TextToPlayer.text = "";
            playerCanInteract = false;
        }
    }

    private void Update()
    {
        if (playerCanInteract && Input.GetKeyDown(KeyCode.E) && !cameraZoom)
        {
            try
            {
                if (!isCollected && GameManager.Instance.AuthUser != null)
                {
                    isCollected = true;

                    StartCoroutine(PlayerCRUD.AddFragmentToPlayer(id.ToString(),
                    () => Debug.Log("Collected SUCCESSFULLY id: " + id),
                    error => Debug.LogError(error)
                    ));
                }
            }
            catch
            {
                Debug.LogWarning("You are playing has guest");
            }


            if (!playerIsInteracting)
            {
                playerIsInteracting = true;
                StartCoroutine(OpenCollectable());
            }

            else
            {
                playerIsInteracting = false;
                StartCoroutine(CloseCollectable());
            }

        }
    }

    private IEnumerator OpenCollectable()
    {

        cameraZoom = true;

        GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().velocity = Vector3.zero;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().enabled = false;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerCam>().enabled = false;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ItemBobbing>().enabled = false;
        GameObject.FindGameObjectWithTag("PlayerInterface").GetComponent<Canvas>().enabled = false;

        StartCoroutine(cameraZoomIn.CameraMovement());

        cameraZoomOut.finalCameraPos = cameraZoomIn.initialCameraPos;
        cameraZoomOut.finalCameraRotation = cameraZoomIn.initialCameraRotation;

        yield return new WaitForSeconds(cameraZoomIn.duration);

        cameraZoom = false;

    }

    private IEnumerator CloseCollectable()
    { 

        cameraZoom = true;

        StartCoroutine(cameraZoomOut.CameraMovement());

        yield return new WaitForSeconds(cameraZoomOut.duration);

        cameraZoom = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().enabled = true;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerCam>().enabled = true;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ItemBobbing>().enabled = true;
        GameObject.FindGameObjectWithTag("PlayerInterface").GetComponent<Canvas>().enabled = true;
    }


}

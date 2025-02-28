using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractableDoor : MonoBehaviour
{

    [SerializeField] private AudioManager openingDoorSFX;
    [SerializeField] private AudioManager closingDoorSFX;

    private Animator doorAnimator;

    public bool playerCanInteract;
    private bool isOpen = false;
    private string currentText = "[E] to Open";

    public bool isLocked = false;

    private void Start()
    {
        doorAnimator = gameObject.GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerObj") && !isLocked)
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
        if (playerCanInteract && Input.GetKeyDown(KeyCode.E))
        {

            if (isOpen)
            {
                StartCoroutine(closingDoorSFX.PlayRandomSoundWithDelay(0.45f));
                currentText = "[E] to Open";
                doorAnimator.SetBool("isOpen", false);
                isOpen = false;
            }
            else
            {
                openingDoorSFX.PlayRandomSoundOnce();
                currentText = "[E] to Close";
                doorAnimator.SetBool("isOpen", true);
                isOpen = true;
            }

            PlayerInterface.Instance.TextToPlayer.text = currentText;

            // Stops interaction when the door is opening or closing
            StartCoroutine(StopInteraction());

        }
    }

    private IEnumerator StopInteraction()
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
        playerCanInteract = false;
        PlayerInterface.Instance.TextToPlayer.text = "";
        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }

}

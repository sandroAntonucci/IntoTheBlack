using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractableDoor : MonoBehaviour
{

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
            Debug.Log("This works");
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
                currentText = "[E] to Open";
                doorAnimator.SetBool("isOpen", false);
                isOpen = false;
            }
            else
            {
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
        yield return new WaitForSeconds(0.65f);
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }

}

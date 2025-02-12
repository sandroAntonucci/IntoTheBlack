using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosetHide : MonoBehaviour
{

    [SerializeField] private GameObject interactionText;

    private bool playerInRange = false;

    private bool playerInsideCloset = false;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            interactionText.SetActive(true);
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            interactionText.SetActive(false);
            playerInRange = false;
        }

    }

    private void EnterCloset()
    {
        playerInsideCloset = true;
        interactionText.SetActive(false);
        anim.Play("Closet_Open");
    }

    private void ExitCloset()
    {
        playerInsideCloset = false;
        interactionText.SetActive(true);
        anim.Play("Closet_Open");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerInRange)
        {

            if (!playerInsideCloset) EnterCloset();
            
            else ExitCloset();

        }
    }

}

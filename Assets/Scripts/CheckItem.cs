using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckItem : MonoBehaviour
{

    [SerializeField] private InteractableObject interactableObject;
    [SerializeField] private string itemName;

    bool playerCanInteract = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerObj"))
        {
            playerCanInteract = true;
            PlayerInterface.Instance.TextToPlayer.text = "[E] to interact";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerObj"))
        {
            playerCanInteract = false;
            PlayerInterface.Instance.TextToPlayer.text = "";
        }
    }

    private void Update()
    {
        if (playerCanInteract && Input.GetKeyDown(KeyCode.E))
        {
            if(PlayerInventory.Instance.currentItem != null && PlayerInventory.Instance.currentItem.GetComponent<PickableItem>().scriptableItem.itemName == itemName)
            {
                Destroy(PlayerInventory.Instance.currentItem);
                PlayerInventory.Instance.currentItem = null;
                interactableObject.Interaction();
            }
            else
            {
                interactableObject.InteractionError();
            }
        }
    }

}


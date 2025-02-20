using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckItem : MonoBehaviour
{

    [SerializeField] private InteractableObject interactableObject;
    [SerializeField] private string[] itemNames;

    bool foundItem = false;
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

            // Searches for the item in the player's inventory
            foreach (string s in itemNames)
            {
                if (PlayerInventory.Instance.currentItem != null && PlayerInventory.Instance.currentItem.GetComponent<PickableItem>().scriptableItem.itemName == s)
                {
                    interactableObject.Interaction(s);
                    Destroy(PlayerInventory.Instance.currentItem);
                    PlayerInventory.Instance.currentItem = null;
                    foundItem = true;
                }
            }

            // If the item is not found in the player's inventory
            if (!foundItem)
            {
                interactableObject.InteractionError();
            }

            // Reset the foundItem variable
            foundItem = false;

        }
    }

}


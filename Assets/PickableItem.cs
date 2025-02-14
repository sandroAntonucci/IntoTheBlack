using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : MonoBehaviour
{

    [SerializeField] private Item scriptableItem;

    [SerializeField] private GameObject itemTag;

    private bool playerCanInteract = false;

    private void Start()
    {
        Instantiate(scriptableItem.itemModel, transform.position, Quaternion.identity, transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerObj"))
        {
            itemTag.SetActive(true);
            playerCanInteract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerObj"))
        {
            itemTag.SetActive(false);
            playerCanInteract = false;
        }
    }

    private void Update()
    {
        if (playerCanInteract && Input.GetKeyDown(KeyCode.E))
        {
            //PlayerInventory.Instance.AddItem(scriptableItem);
            Destroy(gameObject);
        }
    }
}






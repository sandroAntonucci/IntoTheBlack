using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : MonoBehaviour
{

    [SerializeField] private Item scriptableItem;

    public GameObject instantiatedItem;

    public bool playerCanInteract = false;


    private void Start()
    {
        instantiatedItem = Instantiate(scriptableItem.itemModel, transform.position, Quaternion.identity, transform);
        instantiatedItem.AddComponent<MeshCollider>().enabled = false;
        instantiatedItem.GetComponent<MeshCollider>().convex = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerObj"))
        {
            PlayerInterface.Instance.TextToPlayer.text = "[E] to pick up";
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
            gameObject.GetComponent<BoxCollider>().enabled = false;
            PlayerInventory.Instance.AddItem(gameObject);
            PlayerInterface.Instance.TextToPlayer.text = "";
            playerCanInteract = false;
        }
    }

    public IEnumerator DroppedItem()
    {
        yield return new WaitForSeconds(1f);
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }
}






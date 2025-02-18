using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{

    public GameObject currentOrientation;
    public GameObject itemHolder;

    public static PlayerInventory Instance { get; private set; }

    public void Update()
    {
       if (Input.GetKeyDown(KeyCode.G))
       {
            DropItem();
       }
    }

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject currentItem;

    public void DropItem()
    {

        if (currentItem != null)
        {

            PickableItem pickableItem = currentItem.GetComponent<PickableItem>();

            currentItem.transform.parent = null;

            pickableItem.instantiatedItem.GetComponent<MeshCollider>().enabled = true;


            currentItem.GetComponent<Rigidbody>().isKinematic = false;
            currentItem.GetComponent<Rigidbody>().AddForce(currentOrientation.transform.forward  * 8, ForceMode.Impulse);

            StartCoroutine(pickableItem.DroppedItem());

            currentItem = null;
        }

    }

    public void AddItem(GameObject item)
    {
        if (item != null)
        {
            currentItem = item;

            // Set as child of itemHolder
            currentItem.transform.parent = itemHolder.transform;

            // Reset position and rotation relative to parent
            currentItem.transform.localPosition = new Vector3(-0.55f, -0.15f, 1f);
            currentItem.transform.localRotation = Quaternion.Euler(0, 0, 0);

            currentItem.GetComponent<Rigidbody>().isKinematic = true;

            // Disable physics
            PickableItem pickableItem = currentItem.GetComponent<PickableItem>();
            pickableItem.instantiatedItem.GetComponent<MeshCollider>().enabled = false;

            pickableItem.instantiatedItem.transform.localPosition = Vector3.zero;
            pickableItem.instantiatedItem.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }


}

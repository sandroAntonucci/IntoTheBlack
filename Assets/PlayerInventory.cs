using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{

    public GameObject currentOrientation;

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

            currentItem.transform.parent = null;

            currentItem.GetComponent<Rigidbody>().isKinematic = false;
            currentItem.GetComponent<MeshCollider>().enabled = true;
            
            currentItem.GetComponent<Rigidbody>().AddForce(currentOrientation.transform.forward  * 8, ForceMode.Impulse);

            currentItem = null;
        }


    }


}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(fileName = "PickableItem", menuName = "ScriptableObjects/PickableItem", order = 1)]

public class Item : ScriptableObject
{

    public string itemName;
    public GameObject itemModel;

}

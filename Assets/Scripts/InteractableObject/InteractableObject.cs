using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    
    public abstract void Interaction(string itemName);
    public abstract void InteractionError();

}

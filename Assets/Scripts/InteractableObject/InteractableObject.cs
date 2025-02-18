using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    
    public abstract void Interaction();
    public abstract void InteractionError();

}

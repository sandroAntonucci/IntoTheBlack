using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelDetector : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerObj"))
        {
            GameManager.instance.GoToNextLevel();
        }
    }


}

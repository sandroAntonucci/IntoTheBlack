using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveRecordTime : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerObj"))
        {
            try
            {
                if (GameManager.Instance.AuthUser != null)
                {
                    StartCoroutine(PlayerCRUD.UpdateRecordTime(
                    () => Debug.Log("Record time saved"),
                    error => Debug.LogError(error)
                    ));
                }
            }
            catch
            {
                Debug.LogWarning("You are playing has guest");
            }

        }
    }
}

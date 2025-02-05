using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FrightBehaviour : MonoBehaviour
{
    private const string PlayerTag = "Player";
    private const float time = 1f;
    public Camera frightCamera;

    private IEnumerator ActivateEnemyCamera()
    {
        frightCamera.gameObject.SetActive(true);
        Time.timeScale = 0;

        // En lugar de WaitForSeconds, utilizar WaitForSecondsRealtime para que no se para infinito.
        yield return new WaitForSecondsRealtime(time);

        Time.timeScale = 1;
        frightCamera.gameObject.SetActive(false);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(PlayerTag))
        {
            StopCoroutine(ActivateEnemyCamera());
            StartCoroutine(ActivateEnemyCamera());
        }
    }
}

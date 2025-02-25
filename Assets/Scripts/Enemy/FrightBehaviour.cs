using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FrightBehaviour : MonoBehaviour
{

    [SerializeField] private AudioSource jumpscareSFX;

    private const string PlayerTag = "PlayerObj";
    private const float time = 2f;
    public Camera frightCamera;

    private IEnumerator ActivateEnemyCamera()
    {
        frightCamera.gameObject.SetActive(true);

        jumpscareSFX.Play();

        Time.timeScale = 0;

        // En lugar de WaitForSeconds, utilizar WaitForSecondsRealtime para que no se para infinito.
        yield return new WaitForSecondsRealtime(time);

        Time.timeScale = 1;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag(PlayerTag))
        {
            StopCoroutine(ActivateEnemyCamera());
            StartCoroutine(ActivateEnemyCamera());
        }
    }
}

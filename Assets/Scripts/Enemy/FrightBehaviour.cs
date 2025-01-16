using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrightBehaviour : MonoBehaviour
{
    private const float time = 1f;

    public GameObject target;
    public Camera frightCamera;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(ActivateEnemyCamera());
        }
    }

    private IEnumerator ActivateEnemyCamera()
    {
        frightCamera.gameObject.SetActive(true);
        Time.timeScale = 0;

        // En lugar de WaitForSeconds, utilizar WaitForSecondsRealtime para que no se para infinito.
        yield return new WaitForSecondsRealtime(time);

        Time.timeScale = 1;
        frightCamera.gameObject.SetActive(false);
    }

    private Camera GetTargetCamera(GameObject target)
    {
        return target.GetComponent<Camera>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == target.tag)
        {
            StartCoroutine(ActivateEnemyCamera());
        }
    }
}

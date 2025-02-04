using System.Collections;
using UnityEngine;

public class MoveMenuCamera : MonoBehaviour
{

    private Camera mainCamera;

    public bool isZooming = false;

    [SerializeField] private GameObject finalPositionReference;
    [SerializeField] private GameObject initialPositionReference;

    public Vector3 initialCameraPos;
    public Vector3 finalCameraPos;

    public Vector3 initialCameraRotation;
    public Vector3 finalCameraRotation;

    public float duration;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    public IEnumerator CameraMovement()
    {

        if (finalPositionReference != null)
        {
            finalCameraPos = finalPositionReference.transform.position;
            finalCameraRotation = finalPositionReference.transform.rotation.eulerAngles;
        }

        if (initialPositionReference != null)
        {
            initialCameraPos = initialPositionReference.transform.position;
            initialCameraRotation = initialPositionReference.transform.rotation.eulerAngles;
        }

        else
        {
            initialCameraPos = GameObject.FindGameObjectWithTag("CameraPosition").transform.position;
            initialCameraRotation = Quaternion.Euler(mainCamera.transform.rotation.eulerAngles).eulerAngles;
        }

        isZooming = true;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            mainCamera.transform.position = Vector3.Lerp(initialCameraPos, finalCameraPos, t);
            mainCamera.transform.rotation = Quaternion.Euler(Vector3.Lerp(initialCameraRotation, finalCameraRotation, t));

            elapsedTime += Time.deltaTime;
            yield return null;

            mainCamera.transform.position = finalCameraPos;
            mainCamera.transform.rotation = Quaternion.Euler(finalCameraRotation);
        }

        isZooming = false;
    }
}

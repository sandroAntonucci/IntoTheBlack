using System.Collections;
using UnityEngine;

public class MoveMenuCamera : MonoBehaviour
{

    private Camera mainCamera;

    [SerializeField] private GameObject finalPositionReference;

    public bool getsRotationFromCamera = false;

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

            if (getsRotationFromCamera)
                finalCameraRotation = mainCamera.transform.rotation.eulerAngles;
            else
                finalCameraRotation = finalPositionReference.transform.rotation.eulerAngles;
        }

        initialCameraPos = GameObject.FindGameObjectWithTag("CameraPosition").transform.position;
        initialCameraRotation = Quaternion.Euler(mainCamera.transform.rotation.eulerAngles).eulerAngles;

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
    }
}

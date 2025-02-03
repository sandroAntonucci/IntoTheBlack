using System.Collections;
using UnityEngine;

public class MoveMenuCamera : MonoBehaviour
{
    public Vector3 initialCameraPos;
    public Vector3 finalCameraPos;
    public Vector3 initialCameraRotation;
    public Vector3 finalCameraRotation;
    public float duration;
    public IEnumerator CameraMovement()
    {
        yield return new WaitForSeconds(0.6f);

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            transform.position = Vector3.Lerp(initialCameraPos, finalCameraPos, t);
            transform.rotation = Quaternion.Euler(Vector3.Lerp(initialCameraRotation, finalCameraRotation, t));

            elapsedTime += Time.deltaTime;
            yield return null;
            transform.position = finalCameraPos;
            transform.rotation = Quaternion.Euler(finalCameraRotation);
        }
    }
}

using UnityEngine;

public class CameraBob : MonoBehaviour
{
    public Transform player;  // Assign the player's transform
    public float bobFrequency = 6f; // Speed of the bobbing effect
    public float bobAmplitude = 0.1f; // Height of the bobbing effect
    private float timer = 0f;
    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.localPosition; // Store the original local position of the camera
    }

    void Update()
    {
        if (player == null) return;

        // Get player's movement speed
        float speed = player.GetComponent<Rigidbody>().velocity.magnitude;

        if (speed > 0.1f) // Only bob if the player is moving
        {
            timer += Time.deltaTime * bobFrequency * speed; // Scale bobbing with speed
            float bobOffset = Mathf.Sin(timer) * bobAmplitude;
            transform.localPosition = originalPosition + new Vector3(0, bobOffset, 0);
        }
        else
        {
            timer = 0; // Reset when stopped to avoid jerky behavior
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime * 5f);
        }
    }
}

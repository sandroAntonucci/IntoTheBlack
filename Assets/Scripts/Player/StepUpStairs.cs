using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    public float groundRayDistance = 1.2f; // Distance for ground check
    public float extraGravity = 20f; // Additional gravity to keep player grounded
    public LayerMask groundLayer; // Set this to detect ground layers

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        RaycastHit hit;
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, groundRayDistance, groundLayer);

        Debug.Log(isGrounded);

        if (!isGrounded)
        {
            rb.AddForce(Vector3.down * extraGravity, ForceMode.Acceleration);
        }
    }

    void OnDrawGizmos()
    {
        // Visualizing the ray in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundRayDistance);
    }
}

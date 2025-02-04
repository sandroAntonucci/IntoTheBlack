using UnityEngine;
using UnityEditor;

public class ApplyPhysicsMaterialInEditor : MonoBehaviour
{
    public PhysicMaterial newPhysicsMaterial; // Assign in the Inspector

    [ContextMenu("Apply Physics Material to Children")]
    void ApplyMaterial()
    {
        if (newPhysicsMaterial == null)
        {
            Debug.LogWarning("No physics material assigned!");
            return;
        }

        Collider[] colliders = GetComponentsInChildren<Collider>(); // Get all colliders

        foreach (Collider collider in colliders)
        {
            collider.sharedMaterial = newPhysicsMaterial; // Assign the physics material
        }

        Debug.Log("Physics material applied to all children.");
    }
}

using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ObjectRespawn : MonoBehaviour
{
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    [Tooltip("The GameObject colliders that should trigger respawning")]
    public Collider[] RespawnColliders;

    private void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    private void OnCollisionStay(Collision collision)
    {
        foreach (Collider col in RespawnColliders)
        {
            if (GameObject.ReferenceEquals(collision.collider, col))
            {
                if (gameObject.TryGetComponent<OVRGrabbable>(out OVRGrabbable grabbable))
                {
                    if (grabbable.isGrabbed)
                    {
                        return;
                    }
                }
                transform.position = originalPosition;
                transform.rotation = originalRotation;
            }
        }
    }
}

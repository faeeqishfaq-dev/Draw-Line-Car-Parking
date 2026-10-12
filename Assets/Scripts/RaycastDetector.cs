using UnityEngine;
using UnityEngine.InputSystem;

public struct ContactInfo
{
    public bool contacted;
    public Vector3 point;
    public Collider collider;
    public Transform transform;
}

public class RaycastDetector
{
    public ContactInfo RayCast(int layer)
    {
        ContactInfo contactInfo = new ContactInfo
        {
            contacted = false,
            point = Vector3.zero,
            collider = null,
            transform = null
        };

        // No pointer available
        if (Pointer.current == null)
            return contactInfo;

        // No Main Camera
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
            return contactInfo;

        // Works with mouse + mobile touch
        Vector2 screenPosition = Pointer.current.position.ReadValue();

        Ray ray = mainCamera.ScreenPointToRay(screenPosition);

        bool hit = Physics.Raycast(
            ray,
            out RaycastHit hitInfo,
            Mathf.Infinity,
            1 << layer
        );

        if (hit)
        {
            contactInfo.contacted = true;
            contactInfo.point = hitInfo.point;
            contactInfo.collider = hitInfo.collider;
            contactInfo.transform = hitInfo.transform;
        }

        return contactInfo;
    }
}
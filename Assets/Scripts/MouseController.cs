using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    private static MouseController instance;

    // Set in inspector
    [SerializeField] private LayerMask mousePlaneLayerMask;


    private void Awake()
    {
        instance = this; // Cheap singleton.
    }

    /// <summary>
    ///  Gets the point clicked on the appropriate masked layer via a raytrace.
    /// </summary>
    /// <returns>Vector3 of point clicked.</returns>
    public static Vector3 GetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, instance.mousePlaneLayerMask);
        return raycastHit.point;
    }
}

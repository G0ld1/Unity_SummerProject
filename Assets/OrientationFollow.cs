using UnityEngine;

public class OrientationFollow : MonoBehaviour
{
     public Transform cam;

    void LateUpdate()
    {
        Vector3 dir = cam.forward;
        dir.y = 0;
        transform.forward = dir.normalized;
    }
}

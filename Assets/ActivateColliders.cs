using UnityEngine;

public class ActivateColliders : MonoBehaviour
{
     public Collider[] collidersToEnable;

    public void EnableColliders()
    {
        foreach (var col in collidersToEnable)
        {
            col.enabled = true;
        }
    }
    public void DisableColliders()
    {
        foreach (var col in collidersToEnable)
        {
            col.enabled = false;
        }
    }
}

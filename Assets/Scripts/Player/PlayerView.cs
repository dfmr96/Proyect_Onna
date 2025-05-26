using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Transform glassesTransform;

    public void UpdateVisuals(Vector3 aimDirection)
    {
        if (aimDirection.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(aimDirection);
            if (glassesTransform != null)
            {
                glassesTransform.localRotation = Quaternion.Euler(90, -90, transform.rotation.eulerAngles.y);
            }
        }
    }
}
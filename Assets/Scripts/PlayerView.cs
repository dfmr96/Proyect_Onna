using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerView : MonoBehaviour
{
    [SerializeField] private Transform glassesTransform;
    private CharacterController _characterController;

    public void Initialize()
    {
        _characterController = GetComponent<CharacterController>();
    }

    public void Move(Vector3 direction, float speed)
    {
        if (direction.sqrMagnitude > 0.01f)
        {
            _characterController.Move(direction * (speed * Time.deltaTime));
        }
    }

    public void Rotate(Vector3 aimDirection)
    {
        if (aimDirection.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(aimDirection);
            glassesTransform.localRotation = Quaternion.Euler(90, -90, transform.rotation.eulerAngles.y);
        }
    }
}
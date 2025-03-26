using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour, Player_Input.IPlayerActions
{
    private Vector3 _direction = Vector2.zero;
    private CharacterController _characterController = null;
    [SerializeField] private float speed = 0;
    [SerializeField] private Transform glassesTransform;
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_direction != Vector3.zero)
        {
            _characterController.Move(_direction * (speed * Time.deltaTime));
            Quaternion targetRotation = Quaternion.LookRotation(_direction);
            transform.rotation = targetRotation;
            targetRotation = Quaternion.Euler(90, -90 , targetRotation.y);
            glassesTransform.localRotation = targetRotation;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 readVector = context.ReadValue<Vector2>();
        Debug.Log(readVector);
        Vector3 toConvert = new Vector3(readVector.x, 0, readVector.y);
        _direction = IsoVectorConvert(toConvert);
    }
    
    private Vector3 IsoVectorConvert(Vector3 vector)
    {
        Quaternion rotation = Quaternion.Euler(0,45f,0);
        Matrix4x4 isoMatrix = Matrix4x4.Rotate(rotation);
        Vector3 result = isoMatrix.MultiplyPoint3x4(vector);
        return result;
    }
}

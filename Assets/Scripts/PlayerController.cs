using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour, Player_Input.IPlayerActions
{
    [SerializeField] private float speed = 0;
    [SerializeField] private Transform glassesTransform;
    private Vector3 _direction = Vector3.zero;
    private Vector3 _aimDirection = Vector3.zero;
    private CharacterController _characterController = null;
    private bool isFiring = false;
    private Vector3 mouseWorldPos;

    [SerializeField] private PlayerInput _playerInput;
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }
    void Update()
    {
        if (_direction != Vector3.zero)
        {
            _characterController.Move(_direction * (speed * Time.deltaTime));
        }
        
        if (_aimDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_aimDirection);
            transform.rotation = targetRotation;
            targetRotation = Quaternion.Euler(90, -90 , targetRotation.y);
            glassesTransform.localRotation = targetRotation;
        }
        
        Debug.DrawLine(transform.position, transform.position + transform.forward * 10, Color.green);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 readVector = context.ReadValue<Vector2>();
        Vector3 toConvert = new Vector3(readVector.x, 0, readVector.y);
        _direction = Utils.IsoVectorConvert(toConvert);
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        bool readButton = context.ReadValueAsButton();
        isFiring = readButton;
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        Vector3 readValue = context.ReadValue<Vector2>();

        if (Camera.main != null)
        {
            if (_playerInput.currentControlScheme == "Keyboard&Mouse")
            {
                Ray ray = Camera.main.ScreenPointToRay(readValue);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    mouseWorldPos = hit.point;
                    mouseWorldPos.y = 0;
                    _aimDirection = mouseWorldPos - new Vector3(transform.position.x, 0, transform.position.z);
                    _aimDirection.Normalize();
                }
            }

            if (_playerInput.currentControlScheme == "Gamepad")
            {
                Debug.Log(readValue);
                Vector3 toConvert = new Vector3(readValue.x, 0, readValue.y);
                _aimDirection = Utils.IsoVectorConvert(toConvert);
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(mouseWorldPos, 0.5f);
    }
}

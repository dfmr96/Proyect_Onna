using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour, Player_Input.IPlayerActions
{
    [SerializeField] private WeaponController weaponController = null;
    [SerializeField] private PlayerInput playerInput;

    private PlayerView _playerView;
    private PlayerModel _playerModel;
    private Vector3 _direction = Vector3.zero;
    private Vector3 _aimDirection = Vector3.zero;
    private Vector3 _mouseWorldPos;
    private Camera _mainCamera;

    void Awake()
    {
        _mainCamera = Camera.main;
        _playerModel = GetComponent<PlayerModel>();
        _playerView = GetComponent<PlayerView>();
        _playerView.Initialize();
    }

    void Update()
    {
        _playerView.Move(_direction, _playerModel.Speed);
        _playerView.Rotate(_aimDirection);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 readVector = context.ReadValue<Vector2>();
        _direction = Utils.IsoVectorConvert(new Vector3(readVector.x, 0, readVector.y));
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            weaponController?.Attack();
        }
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        Vector3 readValue = context.ReadValue<Vector2>();

        if (playerInput.currentControlScheme == "Keyboard&Mouse")
        {
            if (_mainCamera != null)
            {
                Ray ray = _mainCamera.ScreenPointToRay(readValue);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    _mouseWorldPos = hit.point;
                    _mouseWorldPos.y = 0;
                    _aimDirection = (_mouseWorldPos - new Vector3(transform.position.x, 0, transform.position.z))
                        .normalized;
                }
            }
        }
        else if (playerInput.currentControlScheme == "Gamepad")
        {
            Debug.Log(readValue);
            Vector3 toConvert = new Vector3(readValue.x, 0, readValue.y);
            _aimDirection = Utils.IsoVectorConvert(toConvert);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(_mouseWorldPos, 0.5f);
    }
}
using Player.Weapon;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        private const float AimRaycastMaxDistance = 100f;
    
        [SerializeField] private WeaponController weaponController = null;
        [SerializeField] private LayerMask groundLayer;

        private CharacterController _characterController;
        private Vector3 _aimDirection = Vector3.forward;
        private PlayerInputHandler _playerInputHandler;
        private Vector3 _direction = Vector3.zero;
        private PlayerModel _playerModel;
        private PlayerInput _playerInput;
        private PlayerView _playerView;
        private Vector3 _mouseWorldPos;
        private Camera _mainCamera;
    
        void Awake()
        {
            PlayerHelper.SetPlayer(gameObject);
        
            _mainCamera = Camera.main;
            _playerModel = GetComponent<PlayerModel>();
            _playerView = GetComponent<PlayerView>();
            _playerInput = GetComponent<PlayerInput>();
            _playerInputHandler = GetComponent<PlayerInputHandler>();
            _characterController = GetComponent<CharacterController>();
        
            _playerInputHandler.FirePerformed += HandleFire;
        }

        void Update()
        {
            _direction = _playerInputHandler.MovementInput;
            HandleAiming(_playerInputHandler.RawAimInput);
        
            Move(_direction, _playerModel.Speed);
            Rotate(_aimDirection);
        }
    
        private void HandleAiming(Vector2 rawInput)
        {
            if (_playerInput.currentControlScheme == "Keyboard&Mouse")
            {
                Ray ray = _mainCamera.ScreenPointToRay(rawInput);

                if (Physics.Raycast(ray, out RaycastHit hit, AimRaycastMaxDistance, groundLayer))
                {
                    _mouseWorldPos = hit.point;
                    _mouseWorldPos.y = 0;

                    var position = transform.position;
                    Vector3 flatPos = new Vector3(position.x, 0f, position.z);
                    _aimDirection = (_mouseWorldPos - flatPos).normalized;
                }
            }
            else if (_playerInput.currentControlScheme == "Gamepad")
            {
                Vector3 aim = new Vector3(rawInput.x, 0f, rawInput.y);
                _aimDirection = Utils.IsoVectorConvert(aim).normalized;
            }
        }

        private void Move(Vector3 direction, float speed)
        {
            if (direction.sqrMagnitude > 0.01f)
            {
                _characterController.Move(direction * (speed * Time.deltaTime));
            }
        }

        private void Rotate(Vector3 aimDirection)
        {
            if (aimDirection.sqrMagnitude > 0.01f)
            {
                transform.rotation = Quaternion.LookRotation(aimDirection);
            }
        }
    
        private void HandleFire()
        {
            weaponController.Attack();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(_mouseWorldPos, 0.5f);
        }
    }
}
using Dave6.LootShooter.Camera;
using Dave6.LootShooter.Character.Motor;
using Dave6.LootShooter.Input;
using UnityEngine;

namespace Dave6.LootShooter.Samples.Character
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class PrototypeCharacterController : MonoBehaviour
    {
        ICharacterInput _Input;
        ThirdPersonCamera _Camera;
        BaseMotor _Motor;
        public void Initialize(ICharacterInput input, ThirdPersonCamera camera)
        {
            _Input = input; _Camera = camera;
        }
        void Awake()
        {
            _Motor = GetComponent<BaseMotor>();
        }
        void Update()
        {
            if (_Input == null || _Camera == null) return;
            Move();
            Actions();
        }
        void Move()
        {
            Vector2 input = _Input.Move;
            Vector3 forward = _Camera.Forward;
            Vector3 right = _Camera.Right;
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();
            Vector3 direction = forward * input.y + right * input.x;

            if (direction.sqrMagnitude > 0f) direction.Normalize();
            _Motor.SetMoveDirection(direction);

            bool isPressed = input.sqrMagnitude > 0f;
            if (isPressed) Debug.Log("이동 키 눌림");

            float speed = isPressed ? (_Input.Sprint.IsPressed ? 5.5f : 2f) : 0;
            _Motor.SetTargetSpeed(speed);
            if (_Input.Jump.WasPressedThisFrame) _Motor.TryJump();
        }
        void Actions()
        {
            if (_Input.Fire.WasReleasedThisFrame) Debug.Log($"Release Attack");
            if (_Input.Crouch.WasReleasedThisFrame) Debug.Log($"Release Crouch");
        }
    }
}

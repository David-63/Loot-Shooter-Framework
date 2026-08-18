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
            // 카메라의 상하 회전은 이동 방향에 영향을 주지 않음
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
            // Vector3 velocity = direction * speed;
            // _CharacterController.Move( velocity * Time.deltaTime );
        }
        // void ApplyGravity()
        // {
        //     if (_Input.Jump.WasPressedThisFrame) Debug.Log($"Jump Input! Grounded: {_CharacterController.isGrounded}");
        //     if (_CharacterController.isGrounded)
        //     {
        //         if (_VerticalVelocity < 0f) _VerticalVelocity = -2f;
        //         if (_Input.Jump.WasPressedThisFrame)
        //         {
        //             _VerticalVelocity = Mathf.Sqrt( _JumpHeight * -2f * _Gravity );
        //         }
        //     }
        //     _VerticalVelocity += _Gravity * Time.deltaTime;
        //     _CharacterController.Move( Vector3.up * _VerticalVelocity * Time.deltaTime );
        // }
        void Actions()
        {
            if (_Input.Fire.WasReleasedThisFrame) Debug.Log($"Release Attack");
            if (_Input.Crouch.WasReleasedThisFrame) Debug.Log($"Release Crouch");
        }
    }
}

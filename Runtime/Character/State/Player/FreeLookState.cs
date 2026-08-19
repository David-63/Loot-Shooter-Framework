using Dave6.LootShooter.Foundation.State;
using UnityEngine;

namespace Dave6.LootShooter.Character.States
{
    public sealed class FreeLookState : BaseState<CharacterAgent>
    {
        public FreeLookState(CharacterAgent controller) : base(controller) {}
        public override void OnEnter()
        {
            Debug.Log("Entering FreeLook Locomotion");
        }
        public override void Update()
        {
            HandleJump();
            HandleMovement();
        }
        void HandleJump()
        {
            if (_Controller.Input.Jump.IsPressed)
            {
                _Controller.Motor.TryJump();
            }
        }
        void HandleMovement()
        {
            // 속도 ||=============================================
            Vector2 input = _Controller.Input.Move;
            Vector3 forward = _Controller.Camera.Forward;
            Vector3 right = _Controller.Camera.Right;
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();
            Vector3 direction = forward * input.y + right * input.x;
            if (direction.sqrMagnitude > 0f) direction.Normalize();
            _Controller.Motor.SetMoveDirection(direction);
            // 속도 ||=============================================
            bool isPressed = input.sqrMagnitude > 0f;
            float speed = isPressed ? (_Controller.Input.Sprint.IsPressed ? 5.5f : 2f) : 0;
            _Controller.Motor.SetTargetSpeed(speed);
        }
    }
}
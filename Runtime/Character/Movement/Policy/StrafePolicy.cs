using UnityEngine;

namespace Dave6.LootShooter.Character.Movement.Policy
{
    public sealed class StrafePolicy : IMovementPolicy
    {
        public MoveData ResolveMoveData(CharacterMovement movement, PlayerInputData input)
        {
            var moveDirection = ResolveMoveDirection(movement, input);

            Vector3 forward = input.CameraForward;
            float targetYaw = Mathf.Atan2(forward.x, forward.z) * Mathf.Rad2Deg;
            
            return new MoveData(moveDirection, targetYaw);
        }

        public float GetRotationSpeed(CharacterMovement movement)
        {
            return 360f;
        }

        Vector3 ResolveMoveDirection(CharacterMovement movement, PlayerInputData input)
        {
            Vector3 forward = movement.Forward;
            Vector3 right = movement.Right;

            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            Vector2 moveInput = input.Move;

            Vector3 direction = forward * moveInput.y + right * moveInput.x;
            return direction.sqrMagnitude > 0f ? direction.normalized : Vector3.zero;
        }
    }
}
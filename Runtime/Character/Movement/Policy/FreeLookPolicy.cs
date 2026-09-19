using UnityEngine;

namespace Dave6.LootShooter.Character.Movement.Policy
{
    public sealed class FreeLookPolicy : IMovementPolicy
    {
        public MoveData ResolveMoveData(CharacterMovement movement, PlayerInputData input)
        {
            var moveDirection = ResolveMoveDirection(movement, input);
            float targetYaw;
            if (moveDirection.sqrMagnitude <= 0f) targetYaw = movement.Motor.CurrentYaw;
            else targetYaw = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;

            return new MoveData(moveDirection, targetYaw);
        }
        public float GetRotationSpeed(CharacterMovement movement)
        {
            return 360f;
        }

        Vector3 ResolveMoveDirection(CharacterMovement movement, PlayerInputData input)
        {
            Vector3 forward = input.CameraForward;            
            Vector3 right = input.CameraRight;

            Vector2 moveInput = input.Move;
            Vector3 direction = forward * moveInput.y + right * moveInput.x;
            return direction.sqrMagnitude > 0f ? direction.normalized : Vector3.zero;
        }
    }
}
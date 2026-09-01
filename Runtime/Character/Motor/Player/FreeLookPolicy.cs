using Dave6.LootShooter.Character.Movement;
using UnityEngine;

namespace Dave6.LootShooter.Character.Motor
{
    public sealed class FreeLookPolicy : IMovementPolicy
    {
        public Vector3 ResolveMoveDirection(CharacterMovement movement, Vector2 input)
        {
            Vector3 forward = movement.Camera.Forward;
            Vector3 right = movement.Camera.Right;

            Vector3 direction = forward * input.y + right * input.x;
            return direction.sqrMagnitude > 0f ? direction.normalized : Vector3.zero;
        }
        // 입력이 없어도 이동 방향은 유지되어야함
        public float ResolveTargetYaw(CharacterMovement movement, Vector3 moveDirection)
        {
            if (moveDirection.sqrMagnitude <= 0f) return movement.Motor.CurrentYaw;

            return Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
        }
        public float GetRotationSpeed(CharacterMovement movement)
        {
            return 360f;
        }
    }
}
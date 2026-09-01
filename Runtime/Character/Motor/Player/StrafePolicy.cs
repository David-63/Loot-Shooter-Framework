using Dave6.LootShooter.Character.Movement;
using UnityEngine;

namespace Dave6.LootShooter.Character.Motor
{
    public sealed class StrafePolicy : IMovementPolicy
    {
        public Vector3 ResolveMoveDirection(CharacterMovement movement, Vector2 input)
        {
            Vector3 forward = movement.Agent.transform.forward;
            Vector3 right = movement.Agent.transform.right;

            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            Vector3 direction = forward * input.y + right * input.x;
            return direction.sqrMagnitude > 0f ? direction.normalized : Vector3.zero;
        }

        public float ResolveTargetYaw(CharacterMovement movement, Vector3 moveDirection)
        {
            Vector3 forward = movement.Camera.Forward;
            return Mathf.Atan2(forward.x, forward.z) * Mathf.Rad2Deg;
        }

        public float GetRotationSpeed(CharacterMovement movement)
        {
            return 360f;
        }
    }
}
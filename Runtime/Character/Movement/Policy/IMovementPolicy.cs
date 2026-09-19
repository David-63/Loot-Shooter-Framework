using UnityEngine;

namespace Dave6.LootShooter.Character.Movement.Policy
{
    public interface IMovementPolicy
    {
        MoveData ResolveMoveData(CharacterMovement movement, PlayerInputData input);

        float GetRotationSpeed(CharacterMovement movement);
    }
    public readonly struct MoveData
    {
        public readonly Vector3 moveDirection;
        public readonly float targetYaw;
        public MoveData(Vector3 direction, float yaw)
        {
            moveDirection = direction;
            targetYaw = yaw;
        }
    }
}
using Dave6.LootShooter.Character.Movement;
using UnityEngine;

namespace Dave6.LootShooter.Character.Motor
{
    public interface IMovementPolicy
    {
        Vector3 ResolveMoveDirection(CharacterMovement movement, Vector2 input);
        float ResolveTargetYaw(CharacterMovement movement, Vector3 moveDirection);
        float GetRotationSpeed(CharacterMovement movement);
    }
}
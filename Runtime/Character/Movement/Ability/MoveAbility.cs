using UnityEngine;

namespace Dave6.LootShooter.Character.Movement.Ability
{
    public sealed class MoveAbility : IMovementAbility
    {
        readonly CharacterMovement _Movement;
        public MoveAbility(CharacterMovement movement) => _Movement = movement;

        public void Execute()
        {
            Vector2 input = _Movement.Input.Move;
            _Movement.Motor.SetTargetSpeed(CalculateTargetSpeed(input));

            Vector3 moveDirection = _Movement.Policy.ResolveMoveDirection(_Movement, input);
            _Movement.Motor.SetMoveDirection(moveDirection);

            float targetYaw = _Movement.Policy.ResolveTargetYaw(_Movement, moveDirection);
            _Movement.Motor.SetTargetYaw(targetYaw);
        }

        private float CalculateTargetSpeed(Vector2 input)
        {
            if (input.sqrMagnitude <= 0f) return 0f;
            return _Movement.Input.Sprint.IsPressed ? 5.5f : 1.2f;
        }
    }
}
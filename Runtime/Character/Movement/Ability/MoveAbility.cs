namespace Dave6.LootShooter.Character.Movement.Ability
{
    public sealed class MoveAbility : IMovementAbility
    {
        readonly CharacterMovement _Movement;
        public MoveAbility(CharacterMovement movement) => _Movement = movement;

        public void Execute(PlayerInputData input)
        {
            var data = _Movement.Policy.ResolveMoveData(_Movement, input);

            _Movement.Motor.SetMoveDirection(data.moveDirection);
            _Movement.Motor.SetTargetYaw(data.targetYaw);

            _Movement.Motor.SetTargetSpeed(CalculateTargetSpeed(input));
        }

        private float CalculateTargetSpeed(PlayerInputData input)
        {
            if (input.Move.sqrMagnitude <= 0f) return 0f;
            return input.Sprint ? 6.5f : 2f;
        }
    }
}
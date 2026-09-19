namespace Dave6.LootShooter.Character.Movement.Ability
{
    public sealed class JumpAbility : IMovementAbility
    {
        readonly CharacterMovement _Movement;
        public JumpAbility(CharacterMovement movement) => _Movement = movement;

        public void Execute(PlayerInputData input)
        {
            if (!input.Jump) return;
            _Movement.Motor.TryJump();
        }
    }
}
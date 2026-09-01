namespace Dave6.LootShooter.Character.Movement.Ability
{
    public sealed class JumpAbility : IMovementAbility
    {
        readonly CharacterMovement _Movement;
        public JumpAbility(CharacterMovement movement) => _Movement = movement;

        public void Execute()
        {
            if (!_Movement.Input.Jump.WasPressedThisFrame) return;
            _Movement.Motor.TryJump();
        }
    }
}
using Dave6.LootShooter.Foundation.State;

namespace Dave6.LootShooter.Character.Movement.States
{
    public sealed class StrafeState : BaseState<CharacterMovement>
    {
        public StrafeState(CharacterMovement controller) : base(controller) {}
        public override void OnEnter()
        {
            _Controller.SetMovementMode(MovementMode.Strafe);
        }
    }
}
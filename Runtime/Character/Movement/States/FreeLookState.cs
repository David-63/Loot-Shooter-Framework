using Dave6.LootShooter.Foundation.State;

namespace Dave6.LootShooter.Character.Movement.States
{
    public sealed class FreeLookState : BaseState<CharacterMovement>
    {
        public FreeLookState(CharacterMovement controller) : base(controller) {}

        public override void OnEnter()
        {
            _Controller.SetMovementMode(MovementMode.FreeLook);
        }
    }
}
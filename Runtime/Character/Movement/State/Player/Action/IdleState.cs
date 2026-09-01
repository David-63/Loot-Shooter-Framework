using Dave6.LootShooter.Foundation.State;

namespace Dave6.LootShooter.Character.States
{
    public sealed class IdleState : BaseState<CharacterAgent>
    {
        public IdleState(CharacterAgent controller) : base(controller) {}
    }
}
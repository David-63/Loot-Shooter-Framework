namespace Dave6.LootShooter.Foundation.State
{
    public interface ITransition
    {
        IState To { get; }
        IPredicate Condition { get; }
    }
}
namespace Dave6.LootShooter.Input
{
    public interface IInputActionState
    {
        bool IsPressed { get; }
        bool WasPressedThisFrame { get; }
        bool WasReleasedThisFrame { get; }
    }
}

namespace Dave6.LootShooter.Input
{
    /// <summary>
    /// not used
    /// </summary>
    public interface IInputActionBehavior
    {
        bool IsActive { get; }
        void Update();
        void Reset();
    }

    // public sealed class HoldInputBehaviour : IInputActionBehavior
    // {
    //     readonly IInputActionState _State;
    //     public bool IsActive => _State.IsPressed;
    //     public HoldInputBehaviour(IInputActionState state) => _State = state;
    //     public void Update() {}
    //     public void Reset() {}
    // }
    // public sealed class ToggleInputBehavior : IInputActionBehavior
    // {
    //     readonly IInputActionState _State;
    //     bool _Active;
    //     public bool IsActive => _Active;
    //     public ToggleInputBehavior(IInputActionState state) => _State = state;

    //     public void Update()
    //     {
    //         if (_State.WasPressedThisFrame)
    //             _Active = !_Active;
    //     }

    //     public void Reset()
    //     {
    //         _Active = false;
    //     }
    // }
    // public sealed class TapInputBehavior : IInputActionBehavior
    // {
    //     readonly IInputActionState _State;
    //     public bool IsActive => _State.WasPressedThisFrame;

    //     public TapInputBehavior(IInputActionState state) => _State = state;
    // }
}

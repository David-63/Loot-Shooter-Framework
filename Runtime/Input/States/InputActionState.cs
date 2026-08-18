using UnityEngine.InputSystem;

namespace Dave6.LootShooter.Input
{
    public sealed class InputActionState : IInputActionState
    {
        readonly InputAction _Action;
        public bool IsPressed => _Action.IsPressed();
        public bool WasPressedThisFrame => _Action.WasPressedThisFrame();
        public bool WasReleasedThisFrame => _Action.WasReleasedThisFrame();

        public InputActionState(InputAction action) => _Action = action;
    }
}

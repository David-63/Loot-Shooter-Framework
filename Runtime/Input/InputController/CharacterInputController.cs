using UnityEngine;

namespace Dave6.LootShooter.Input
{
    public sealed class CharacterInput : ICharacterInput
    {
        readonly LootShooterInput _Input;

        public Vector2 Move => _Input.Character.Move.ReadValue<Vector2>();
        public Vector2 Look => _Input.Character.Look.ReadValue<Vector2>();
        public IInputActionState Jump { get; }
        public IInputActionState Crouch { get; }
        public IInputActionState Sprint { get; }

        public IInputActionState Interact { get; }

        public IInputActionState Fire { get; }
        public IInputActionState Aim { get; }
        public IInputActionState Reload { get; }

        public CharacterInput(LootShooterInput input)
        {
            _Input = input;
            Jump = new InputActionState(_Input.Character.Jump);
            Crouch = new InputActionState(_Input.Character.Crouch);
            Sprint = new InputActionState(_Input.Character.Sprint);

            Interact = new InputActionState(_Input.Character.Interact);

            Fire = new InputActionState(_Input.Character.Fire);
            Aim = new InputActionState(_Input.Character.Aim);
            Reload = new InputActionState(_Input.Character.Reload);
        }
    }
}

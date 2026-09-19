using UnityEngine;

namespace Dave6.LootShooter.Character.Movement.Ability
{
    /// <summary>
    /// 압력을 확인하고, Crouch 전환 결정
    /// </summary>
    public sealed class CrouchAbility : IMovementAbility
    {
        readonly CharacterMovement _Movement;

        bool _WasPressed;
        public CrouchAbility(CharacterMovement movement) => _Movement = movement;

        public void Execute(PlayerInputData input)
        {
            var crouching = input.Crouch;
            if (crouching == _WasPressed) return;
            Apply(crouching);
        }
        public void Apply(bool crouching)
        {
            if (crouching == _WasPressed) return;
            _WasPressed = crouching;
            SetCrouch(crouching);
        }

        void SetCrouch(bool crouched)
        {
            _Movement.Model.localScale = crouched ? new Vector3(1, 0.6f, 1) : new Vector3(1, 1, 1);
            _Movement.Motor.SetCrouch(crouched);
        }
    }
}
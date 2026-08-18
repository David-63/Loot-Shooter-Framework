using UnityEngine;

namespace Dave6.LootShooter.Input
{
    public interface ICharacterInput
    {
        Vector2 Move { get; }
        Vector2 Look { get; }

        IInputActionState Jump { get; }
        IInputActionState Sprint { get; }
        IInputActionState Crouch { get; }

        IInputActionState Interact { get; }

        IInputActionState Fire { get; }
        IInputActionState Aim { get; }
        IInputActionState Reload { get; }
    }
}

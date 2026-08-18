using System;
using UnityEngine;

namespace Dave6.LootShooter.Input
{
    public sealed class InputRuntime : IDisposable
    {
        readonly LootShooterInput _Input;
        public ICharacterInput Character { get; }

        public InputRuntime()
        {
            _Input = new LootShooterInput();
            Character = new CharacterInput(_Input);
        }
        public void Dispose()
        {
            DisableCharacter();
            _Input.Dispose();
        }
        public void EnableCharacter()
        {
            Debug.Log("인풋 활성화");
            _Input.Character.Enable();
        }
        public void DisableCharacter()
        {
            _Input.Character.Disable();
        }
    }
}
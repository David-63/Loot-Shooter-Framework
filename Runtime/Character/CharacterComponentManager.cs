using System;
using Dave6.LootShooter.Character.Motor;
using Dave6.LootShooter.Character.Movement;
using UnityEngine;

namespace Dave6.LootShooter.Character
{
    public sealed class CharacterComponentManager : IDisposable
    {
        BaseMotor Motor { get; }
        CharacterMovement Movement { get; }

        public CharacterComponentManager(CharacterAgent agent)
        {
            Motor = agent.GetComponent<BaseMotor>();
            if (Motor == null)
            {
                throw new MissingComponentException($"{agent.name} requires {nameof(BaseMotor)}.");
            }
            Movement = new CharacterMovement(agent, Motor);
        }
        public void Dispose()
        {
            
        }
        public void OnUpdate()
        {
            Movement.OnUpdate();
        }
    }
}
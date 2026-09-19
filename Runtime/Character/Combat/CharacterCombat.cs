using System.Collections.Generic;
using Dave6.LootShooter.Character.Movement;
using Dave6.LootShooter.Foundation.State;

namespace Dave6.LootShooter.Character.Combat
{
    public sealed class CharacterCombat
    {
        readonly CharacterAgent _Agent;
        public CharacterAgent Agent => _Agent;
        readonly List<IMovementAbility> _Abilities = new();
        PlayerInputData _CurrentInput;

        StateMachine _CombatState;

        public CharacterCombat(CharacterAgent agent)
        {
            _Agent = agent;

            SetupAbilities();
            SetupStateMachine();
        }

        public void SetupAbilities()
        {
            
        }

        public void SetupStateMachine()
        {
            
        }

        public void OnUpdate(PlayerInputData input, float deltaTime)
        {
            
        }

    }
}
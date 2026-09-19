using System;
using Dave6.LootShooter.Character.Combat;
using Dave6.LootShooter.Character.Motor;
using Dave6.LootShooter.Character.Movement;
using UnityEngine;

namespace Dave6.LootShooter.Character
{
    public sealed class CharacterComponentManager : IDisposable
    {
        public BaseMotor Motor { get; }
        public CharacterMovement Movement { get; }
        public CharacterCombat Combat { get; }

        public CharacterComponentManager(CharacterAgent agent)
        {
            Motor = agent.GetComponent<BaseMotor>();
            if (Motor == null)
            {
                throw new MissingComponentException($"{agent.name} requires {nameof(BaseMotor)}.");
            }
            Movement = new CharacterMovement(agent, Motor);
            Combat = new (agent);
        }
        public void Dispose()
        {
            
        }
        public void OnUpdate(PlayerInputData input, float deltaTime)
        {
            Movement.OnUpdate(input, deltaTime);
            Combat.OnUpdate(input, deltaTime);
        }

        public void SetCrouched(bool value)
        {
            Motor.SetCrouch(value);
        }
        /*
            1. StateMachine 생성
            2. State 생성
            3. Predicate 로 전환 조건 추가
            4. SetState 로 현재 상태 초기화
        */
        // void SetupStateMachine()
        // {
        //     _ActionSM = new();
        //     var idle = new IdleState(this);
        //     var fire = new FireState(this);
        //     _ActionSM.Any(fire, new FuncPredicate(()=> Input.Fire.IsPressed));
        //     _ActionSM.At(fire, idle, new FuncPredicate(()=> Input.Fire.WasReleasedThisFrame));

        //     _ActionSM.SetState(_ActionSM.GetStateByType(typeof(IdleState)));
        //     if (_DebugStateMachine) _ActionSM.SetDebug(_DebugStateMachine);
        // }
    }
}
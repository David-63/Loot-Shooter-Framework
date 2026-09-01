using System;
using System.Collections.Generic;
using Dave6.LootShooter.Camera;
using Dave6.LootShooter.Character.Motor;
using Dave6.LootShooter.Character.Movement.Ability;
using Dave6.LootShooter.Character.Movement.States;
using Dave6.LootShooter.Foundation.State;
using Dave6.LootShooter.Input;

namespace Dave6.LootShooter.Character.Movement
{
    public enum MovementMode
    {
        FreeLook,
        Strafe
    }
    public sealed class CharacterMovement
    {
        readonly CharacterAgent _Agent;
        public CharacterAgent Agent => _Agent;
        public ICharacterInput Input => _Agent.Input;
        public ThirdPersonCamera Camera => _Agent.Camera;

        // 계산 컴포넌트
        readonly BaseMotor _Motor;
        public BaseMotor Motor => _Motor;

        // 상태 구분
        StateMachine _Locomotion;
        public IMovementPolicy Policy { get; private set; }
        IMovementPolicy _FreeLookPolicy;
        IMovementPolicy _StrafePolicy;
        public MovementMode Mode { get; private set; }
        public void SetMovementMode(MovementMode mode)
        {
            Mode = mode;

            Policy = mode switch
            {
                MovementMode.FreeLook => _FreeLookPolicy,
                MovementMode.Strafe => _StrafePolicy,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        // 모듈식 기능
        readonly List<IMovementAbility> _Abilities = new();


        public CharacterMovement(CharacterAgent agent, BaseMotor motor)
        {
            _Agent = agent;
            _Motor = motor;

            _FreeLookPolicy = new FreeLookPolicy();
            _StrafePolicy = new StrafePolicy();

            SetupAbilities();
            SetupStateMachine();
        }

        public void OnUpdate()
        {
            _Locomotion.Update();

            foreach (var ability in _Abilities)
            {
                ability.Execute();
            }
        }

        void SetupAbilities()
        {
            _Abilities.Add(new MoveAbility(this));
            _Abilities.Add(new JumpAbility(this));
        }
        void SetupStateMachine()
        {
            _Locomotion = new();
            var freelook = new FreeLookState(this);
            var strafe = new StrafeState(this);
            _Locomotion.At(freelook, strafe, new FuncPredicate(()=> Input.Aim.IsPressed));
            _Locomotion.At(strafe, freelook, new FuncPredicate(()=> !Input.Aim.IsPressed));
            
            _Locomotion.SetState(_Locomotion.GetStateByType(typeof(FreeLookState)));
            //if (_DebugStateMachine) _Locomotion.SetDebug(_DebugStateMachine);
        }

    }
}
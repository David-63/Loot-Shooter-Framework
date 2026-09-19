using System;
using System.Collections.Generic;
using Dave6.LootShooter.Character.Motor;
using Dave6.LootShooter.Character.Movement.Ability;
using Dave6.LootShooter.Character.Movement.Policy;
using Dave6.LootShooter.Character.Movement.States;
using Dave6.LootShooter.Foundation.State;
using UnityEngine;

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
        public Transform Model => _Agent.Model;

        // 모듈식 기능
        readonly List<IMovementAbility> _Abilities = new();

        PlayerInputData _CurrentInput;

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

        public Vector3 Forward => _Agent.transform.forward;
        public Vector3 Right => _Agent.transform.right;


        CrouchAbility _CrouchAbility;


        public CharacterMovement(CharacterAgent agent, BaseMotor motor)
        {
            _Agent = agent;
            _Motor = motor;

            _FreeLookPolicy = new FreeLookPolicy();
            _StrafePolicy = new StrafePolicy();

            SetupAbilities();
            SetupStateMachine();
        }

        public void OnUpdate(PlayerInputData input, float deltaTime)
        {
            _CurrentInput = input;
            _Locomotion.Update();

            foreach (var ability in _Abilities)
            {
                ability.Execute(input);
            }

            _Motor.Simulate(deltaTime);
        }

        void SetupAbilities()
        {
            _Abilities.Add(new MoveAbility(this));
            _Abilities.Add(new JumpAbility(this));

            _CrouchAbility = new CrouchAbility(this);
            _Abilities.Add(_CrouchAbility);
        }
        void SetupStateMachine()
        {
            _Locomotion = new();
            var freelook = new FreeLookState(this);
            var strafe = new StrafeState(this);
            _Locomotion.At(freelook, strafe, new FuncPredicate(()=> _CurrentInput.Aim));
            _Locomotion.At(strafe, freelook, new FuncPredicate(()=> !_CurrentInput.Aim));
            
            _Locomotion.SetState(_Locomotion.GetStateByType(typeof(FreeLookState)));
        }

        #region API for network

        public bool IsCrouched => Motor.IsCrouched;
        public void ApplyCrouch(bool crouched)
        {
            _CrouchAbility.Apply(crouched);
        }
        #endregion
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dave6.LootShooter.Foundation.State
{
    public class StateMachine
    {
        StateNode _CurrentNode;
        Dictionary<Type, StateNode> _Nodes = new();
        HashSet<ITransition> _AnyTransitions = new();

        //public IState CurrentState => _CurrentNode.State;
        public IState GetStateByType(Type type) => _Nodes[type].State;

        bool _DebugMode = false;

        public void SetDebug(bool enable)
        {
            _DebugMode = enable;
        }

        public void Update()
        {
            var transitions = GetTransition();
            if (transitions != null)
            {
                ChangeState(transitions.To);
            }
            _CurrentNode.State?.Update();
        }

        public void FixedUpdate()
        {
            _CurrentNode.State?.FixedUpdate();
        }
        public void LateUpdate()
        {
            _CurrentNode.State?.LateUpdate();
        }


        ITransition GetTransition()
        {
            foreach (var transition in _AnyTransitions)
            {
                if (!CanTransition(transition.To)) continue;

                if (transition.Condition.Evaluate()) return transition;
            }

            foreach (var transition in _CurrentNode.Transitions)
            {
                if (!CanTransition(transition.To)) continue;

                if (transition.Condition.Evaluate()) return transition;
            }

            return null;
        }
        public void SetState(IState state)
        {
            _CurrentNode = _Nodes[state.GetType()];
            _CurrentNode.State.OnEnter();
        }

        void ChangeState(IState state)
        {
            if (state == _CurrentNode.State) return;

            var previousState = _CurrentNode.State;
            var nextState = _Nodes[state.GetType()].State;

            if (_DebugMode)
            {
                Debug.Log($"[StateMachine] {previousState?.GetType().Name} -> {nextState.GetType().Name}");
            }

            previousState?.OnExit();
            nextState?.OnEnter();
            _CurrentNode = _Nodes[state.GetType()];
        }

        public void AddTransition(IState from, IState to, IPredicate condition)
        {
            GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition);
        }
        public void AddAnyTransition(IState to, IPredicate condition)
        {
            _AnyTransitions.Add(new Transition(GetOrAddNode(to).State, condition));
        }

        StateNode GetOrAddNode(IState state)
        {
            var node = _Nodes.GetValueOrDefault(state.GetType());
            if (node == null)
            {
                node = new StateNode(state);
                _Nodes.Add(state.GetType(), node);
            }
            return node;
        }

        bool CanTransition(IState next)
        {
            var current = _CurrentNode.State;

            // 잠긴 상태
            if (!current.CanBeOverridden()) return false;
            // 특정 상태 필터
            if (current.CanOverrideBy(next)) return true;

            return current.CanExit();
        }


        
        public void At(IState from, IState to, IPredicate condition) => AddTransition(from, to, condition);
        public void Any(IState to, IPredicate condition) => AddAnyTransition(to, condition);

        class StateNode
        {
            public IState State { get; }
            public HashSet<ITransition> Transitions { get; }

            public StateNode(IState state)
            {
                State = state;
                Transitions = new HashSet<ITransition>();
            }

            public void AddTransition(IState to, IPredicate condition)
            {
                Transitions.Add(new Transition(to, condition));
            }
        }
    }
}
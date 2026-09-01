using Dave6.LootShooter.Camera;
using Dave6.LootShooter.Character.Network;
using Dave6.LootShooter.Character.States;
using Dave6.LootShooter.Foundation.State;
using Dave6.LootShooter.Input;
using UnityEngine;

namespace Dave6.LootShooter.Character
{
    public sealed class CharacterAgent : MonoBehaviour
    {
        [SerializeField] bool _DebugStateMachine;
        ICharacterInput _Input;
        public ICharacterInput Input { get => _Input; }
        NetworkAgent _NetworkAgent;
        public NetworkAgent Network => _NetworkAgent;
        ThirdPersonCamera _Camera;
        public ThirdPersonCamera Camera => _Camera;

        CharacterComponentManager _Components;

        StateMachine _ActionSM;

        public void Initialize(ICharacterInput input, ThirdPersonCamera camera, ICharacterNetwork network)
        {
            _Input = input;
            _Camera = camera;
            _NetworkAgent = new();
            _NetworkAgent.Initialize(network);
        }
        void Awake()
        {
            _Components = new CharacterComponentManager(this);
        }

        void Start()
        {
            SetupStateMachine();
        }
        /*
            1. StateMachine 생성
            2. State 생성
            3. Predicate 로 전환 조건 추가
            4. SetState 로 현재 상태 초기화
        */
        void SetupStateMachine()
        {
            _ActionSM = new();
            var idle = new IdleState(this);
            var fire = new FireState(this);
            _ActionSM.Any(fire, new FuncPredicate(()=> Input.Fire.IsPressed));
            _ActionSM.At(fire, idle, new FuncPredicate(()=> Input.Fire.WasReleasedThisFrame));

            _ActionSM.SetState(_ActionSM.GetStateByType(typeof(IdleState)));
            if (_DebugStateMachine) _ActionSM.SetDebug(_DebugStateMachine);
        }
        void Update()
        {
            if (_Input == null || _Camera == null) return;
            _Components.OnUpdate();
            //_LocomotionSM.Update();
            //_ActionSM.Update();
        }
    }
    public readonly struct MoverFrameInput
    {
        public readonly float DeltaTime;
        public readonly float ReferenceYaw;
        public readonly Vector3 CameraForward;
    }
}
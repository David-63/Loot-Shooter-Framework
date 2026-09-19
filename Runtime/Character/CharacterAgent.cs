using Dave6.LootShooter.Camera;
using Dave6.LootShooter.Character.Combat;
using Dave6.LootShooter.Character.Movement;
using Dave6.LootShooter.Character.Network;
using Dave6.LootShooter.Input;
using UnityEngine;

namespace Dave6.LootShooter.Character
{
    public sealed class CharacterAgent : MonoBehaviour
    {
        [SerializeField] bool _DebugStateMachine;
        [SerializeField] Transform _ModelTransform;
        public Transform Model => _ModelTransform;
        ICharacterInput _Input;
        NetworkAgent _NetworkAgent;
        public NetworkAgent Network => _NetworkAgent;
        ThirdPersonCamera _Camera;

        CharacterComponentManager _Components;

        public CharacterMovement Movement => _Components.Movement;
        public CharacterCombat Combat => _Components.Combat;


        //StateMachine _ActionSM;

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

        public void OnNetworkTick(PlayerInputData input, float deltaTime)
        {
            _Components.OnUpdate(input, deltaTime);
        }

        public PlayerInputData CreateInputData()
        {
            return new PlayerInputData
            {
                Move = _Input.Move,
                Jump = _Input.Jump.IsPressed,
                Crouch = _Input.Crouch.IsPressed,
                Sprint = _Input.Sprint.IsPressed,
                Aim = _Input.Aim.IsPressed,
                Fire = _Input.Fire.IsPressed,

                CameraForward = _Camera.Forward,
                CameraRight = _Camera.Right
            };
        }
    }
    
}
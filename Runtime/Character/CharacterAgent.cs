using Dave6.LootShooter.Camera;
using Dave6.LootShooter.Character.Motor;
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
        ThirdPersonCamera _Camera;
        BaseMotor _Motor;
        StateMachine _LocomotionSM;

        public ICharacterInput Input { get => _Input; }
        public ThirdPersonCamera Camera => _Camera;
        public BaseMotor Motor => _Motor;
        

        public void Initialize(ICharacterInput input, ThirdPersonCamera camera)
        {
            _Input = input;
            _Camera = camera;
        }
        void Awake()
        {
            _Motor = GetComponent<BaseMotor>();
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
            _LocomotionSM = new();

            var freelook = new FreeLookState(this);
            var strafe = new StrafeState(this);

            _LocomotionSM.At(freelook, strafe, new FuncPredicate(()=> Input.Aim.IsPressed));
            _LocomotionSM.At(strafe, freelook, new FuncPredicate(()=> !Input.Aim.IsPressed));
            _LocomotionSM.SetState(_LocomotionSM.GetStateByType(typeof(FreeLookState)));
            
            if (_DebugStateMachine) _LocomotionSM.SetDebug(_DebugStateMachine);

        }
        void Update()
        {
            if (_Input == null || _Camera == null) return;
            //Move();
            //Actions();
            _LocomotionSM.Update();
        }
        void Move()
        {
            Vector2 input = _Input.Move;
            Vector3 forward = _Camera.Forward;
            Vector3 right = _Camera.Right;
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();
            Vector3 direction = forward * input.y + right * input.x;

            if (direction.sqrMagnitude > 0f) direction.Normalize();
            _Motor.SetMoveDirection(direction);

            bool isPressed = input.sqrMagnitude > 0f;
            // if (isPressed) Debug.Log("이동 키 눌림");

            float speed = isPressed ? (_Input.Sprint.IsPressed ? 5.5f : 2f) : 0;
            _Motor.SetTargetSpeed(speed);
            if (_Input.Jump.WasPressedThisFrame) _Motor.TryJump();
        }
        void Actions()
        {
            if (_Input.Fire.WasReleasedThisFrame) Debug.Log($"Release Attack");
            if (_Input.Crouch.WasReleasedThisFrame) Debug.Log($"Release Crouch");
        }
    }
    public interface IMovementPolicy
    {
        Vector3 ResolveMoveDirection();
        float ResolveSpeed();
        Vector3 ResolveFacing();
    }
    public readonly struct MoverFrameInput
    {
        public readonly float DeltaTime;
        public readonly float ReferenceYaw;
        public readonly Vector3 CameraForward;
    }
}
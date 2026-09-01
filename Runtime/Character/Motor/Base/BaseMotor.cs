using Dave6.LootShooter.Foundation.Sensor;
using UnityEngine;

namespace Dave6.LootShooter.Character.Motor
{
    [RequireComponent(typeof(CharacterController))]
    public class BaseMotor : MonoBehaviour
    {
        protected BaseMotorContext _Context { get; private set; }
        protected BaseMotorAction _Action { get; private set; }

        [SerializeField]
        protected BaseMotorConfig _Config;

        CharacterController _Controller;
        public PhysicsSensor GroundSensor { get; private set; }

        public bool IsGrounded => _Context.IsGrounded;
        public float HorizontalSpeed => _Context.HorizontalSpeed;
        public float VerticalSpeed => _Context.VerticalSpeed;
        public Vector3 Velocity => _Context.Velocity;
        public float CurrentYaw => _Context.CurrentYaw;

        protected virtual void Awake()
        {
            Initialize();
        }
        protected virtual void Update()
        {
            Simulate(Time.deltaTime);
        }

        protected virtual void Initialize()
        {
            _Context = new BaseMotorContext();
            _Action = new BaseMotorAction(_Config);

            InitializeCharacterController();
            InitializeGroundSensor();
        }

        void InitializeCharacterController()
        {
            _Controller = GetComponent<CharacterController>();

            _Controller.height = _Config.Height;
            _Controller.radius = _Config.Radius;
            _Controller.center = _Config.Center;

            _Controller.stepOffset = _Config.Height * _Config.StepHeightRatio;

            _Controller.skinWidth = _Config.Radius * 0.1f;
        }

        void InitializeGroundSensor()
        {
            GroundSensor = new PhysicsSensor(transform)
            {
                Radius = _Config.Radius,
                LayerMask = ~0,
                TriggerInteraction = QueryTriggerInteraction.Ignore
            };
            GroundSensor.SetDirection(Vector3.down);
        }
        protected virtual void Simulate(float deltaTime)
        {
            _Action.UpdateGrounded(this, _Controller, _Context);

            _Action.UpdateGravity(_Context, deltaTime);
            _Action.UpdateSpeed(_Context, deltaTime);
            _Action.UpdateRotation(_Controller, _Context, deltaTime);
            _Action.UpdateVelocity(_Context);

            _Controller.Move(_Context.Velocity * deltaTime);
        }

        #region Motor API
        public void SetMoveDirection(Vector3 direction)
        {
            if (direction.sqrMagnitude <= 0.0001f)
            {
                _Context.MoveDirection = Vector3.zero;
                return;
            }
            _Context.MoveDirection = direction.normalized;
        }
        public void SetTargetYaw(float yaw)
        {
            _Context.TargetYaw = yaw;
        }

        public void SetTargetSpeed(float speed)
        {
            _Context.TargetSpeed = Mathf.Clamp(speed, 0f, _Config.MaxMoveSpeed);
        }

        public bool TryJump()
        {
            if (!_Context.IsGrounded) return false;

            _Action.Jump(_Context);
            return true;
        }
        #endregion
    }

    /*
        protected virtual void OnValidate()
        {
            if (!gameObject.activeInHierarchy) return;

            ApplyCharacterControllerSettings();
            RecalibrateGroundSensor();
        }

        protected void ApplyCharacterControllerSettings()
        {
            _Controller.center = _Config.Center;
            _Controller.radius = _Config.Radius;
            _Controller.height = _Config.Height;

            _Controller.stepOffset = _Config.Height * _Config.StepHeightRatio;
            _Controller.skinWidth = _Config.Radius * 0.1f;
        }
        protected virtual void RecalibrateGroundSensor()
        {
            GroundSensor.Radius = _Config.Radius;

            GroundSensor.SetOrigin(_Controller.bounds.center);
            GroundSensor.SetDirection(Vector3.down);

            GroundSensor.Length = _Config.Height * (0.5f - _Config.StepHeightRatio) + _Config.Height * _Config.StepHeightRatio;
            GroundSensor.LayerMask = BuildCollisionMask();
        }
        LayerMask BuildCollisionMask()
        {
            int objectLayer = gameObject.layer;
            int mask = Physics.AllLayers;

            for (int i = 0; i < 32; i++)
            {
                if (Physics.GetIgnoreLayerCollision(objectLayer, i))
                    mask &= ~(1 << i);
            }

            int ignoreRaycast = LayerMask.NameToLayer("Ignore Raycast");

            if (ignoreRaycast >= 0)
                mask &= ~(1 << ignoreRaycast);

            return mask;
        }
    */
}

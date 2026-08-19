using Dave6.LootShooter.Input;
using Unity.Cinemachine;
using UnityEngine;

namespace Dave6.LootShooter.Camera
{
    public sealed class ThirdPersonCamera : MonoBehaviour
    {
        ThirdPersonCameraContext _CameraContext;
        [SerializeField] ThirdPersonCameraConfig _CameraConfig;
        ICharacterInput _Input;

        CinemachineCamera _Camera;
        CinemachineThirdPersonFollow _ThirdPersonFollow;

        public Transform Target { get; private set; }

        public Vector3 Forward
        {
            get
            {
                Vector3 forward = Target.forward;
                forward.y = 0f;
                return forward.normalized;
            }
        }

        public Vector3 Right
        {
            get
            {
                Vector3 right = Target.right;
                right.y = 0f;
                return right.normalized;
            }
        }


        void Awake()
        {
            _Camera = GetComponent<CinemachineCamera>();
            _ThirdPersonFollow = GetComponent<CinemachineThirdPersonFollow>();

            if (_Camera == null)
            {
                Debug.LogError($"{nameof(ThirdPersonCamera)} requires " + $"{nameof(CinemachineCamera)}.", this);
                enabled = false;
                return;
            }

            if (_ThirdPersonFollow == null)
            {
                Debug.LogError($"{nameof(ThirdPersonCamera)} requires " + $"{nameof(CinemachineThirdPersonFollow)}.", this);
                enabled = false;
                return;
            }
        }

        public void Initialize(ICharacterInput input)
        {
            _Input = input;

            _CameraContext = new();
            _CameraConfig = new();
            SetupCamera();
        }

        void SetupCamera()
        {
            _Camera.Lens.FieldOfView = _CameraContext.TargetPreset.Fov;
            _ThirdPersonFollow.CameraDistance = _CameraContext.TargetPreset.Distance;
            _ThirdPersonFollow.CameraSide = _CameraContext.TargetPreset.SideLength;
            _ThirdPersonFollow.AvoidObstacles.Enabled = true;
            _ThirdPersonFollow.AvoidObstacles.IgnoreTag = "LocalPlayer";
        }

        public void SetCameraTarget(Transform followTarget)
        {
            Target = followTarget;
            _Camera.Follow = Target;
        }

        void Update()
        {
            if (_Input == null || Target == null) return;
            LookInput(_Input.Look);
        }

        void LateUpdate()
        {
            if (_Input == null || Target == null) return;

            UpdateLook();
        }

        public void LookInput(Vector2 lookDelta)
        {
            if (lookDelta.sqrMagnitude >= 0.0001f)
            {
                _CameraContext.InputYaw += lookDelta.x * _CameraConfig.LookSensitive;
                _CameraContext.InputPitch += lookDelta.y * _CameraConfig.LookSensitive;
            }

            _CameraContext.InputYaw = ClampAngle(_CameraContext.InputYaw, float.MinValue, float.MaxValue);
            _CameraContext.InputPitch = ClampAngle(_CameraContext.InputPitch, _CameraConfig.BottomClamp, _CameraConfig.TopClamp);

            _CameraContext.FinalYaw = _CameraContext.InputYaw;
            _CameraContext.FinalPitch = _CameraContext.InputPitch;
        }

        void UpdateLook()
        {
            Target.rotation = Quaternion.Euler(_CameraContext.FinalPitch, _CameraContext.FinalYaw, 0f);

            // Vector2 look = _Input.Look;

            // _Yaw += look.x * _Sensitivity;
            // _Pitch -= look.y * _Sensitivity;

            // _Pitch = Mathf.Clamp(_Pitch, _MinPitch, _MaxPitch);

            // _Target.rotation = Quaternion.Euler(_Pitch, _Yaw, 0f);
        }

        float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }
    }
}

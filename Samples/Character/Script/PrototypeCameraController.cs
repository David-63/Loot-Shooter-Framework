using Dave6.LootShooter.Input;
using Unity.Cinemachine;
using UnityEngine;

namespace Dave6.LootShooter.Samples.Character
{
    public sealed class PrototypeCameraController : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] Transform _Target;

        [Header("Camera")]
        [SerializeField] float _Sensitivity = 0.1f;
        [SerializeField] float _MinPitch = -80f;
        [SerializeField] float _MaxPitch = 80f;

        [Header("Third Person")]
        [SerializeField] float _CameraDistance = 4f;
        [SerializeField] float _CameraSide = 0.5f;
        [SerializeField] float _VerticalArmLength = 1f;

        ICharacterInput _Input;

        CinemachineCamera _Camera;
        CinemachineThirdPersonFollow _ThirdPersonFollow;

        float _Yaw;
        float _Pitch;

        public Transform Target => _Target;

        public Vector3 Forward
        {
            get
            {
                Vector3 forward = _Target.forward;
                forward.y = 0f;
                return forward.normalized;
            }
        }

        public Vector3 Right
        {
            get
            {
                Vector3 right = _Target.right;
                right.y = 0f;
                return right.normalized;
            }
        }

        public void Initialize(ICharacterInput input)
        {
            _Input = input;
        }

        void Awake()
        {
            _Camera = GetComponent<CinemachineCamera>();
            _ThirdPersonFollow = GetComponent<CinemachineThirdPersonFollow>();

            if (_Camera == null)
            {
                Debug.LogError($"{nameof(PrototypeCameraController)} requires " + $"{nameof(CinemachineCamera)}.", this);
                enabled = false;
                return;
            }

            if (_ThirdPersonFollow == null)
            {
                Debug.LogError($"{nameof(PrototypeCameraController)} requires " + $"{nameof(CinemachineThirdPersonFollow)}.", this);
                enabled = false;
                return;
            }

            SetupCamera();
        }

        void SetupCamera()
        {
            _Camera.Follow = _Target;

            _ThirdPersonFollow.CameraDistance = _CameraDistance;
            _ThirdPersonFollow.CameraSide = _CameraSide;
            _ThirdPersonFollow.VerticalArmLength = _VerticalArmLength;
            _ThirdPersonFollow.AvoidObstacles.Enabled = true;
        }

        void LateUpdate()
        {
            if (_Input == null || _Target == null) return;

            UpdateLook();
        }

        void UpdateLook()
        {
            Vector2 look = _Input.Look;

            _Yaw += look.x * _Sensitivity;
            _Pitch -= look.y * _Sensitivity;

            _Pitch = Mathf.Clamp(_Pitch, _MinPitch, _MaxPitch);

            _Target.rotation = Quaternion.Euler(_Pitch, _Yaw, 0f);
        }
    }
}

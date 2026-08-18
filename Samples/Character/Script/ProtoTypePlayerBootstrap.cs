using Dave6.LootShooter.Camera;
using Dave6.LootShooter.Input;
using UnityEngine;

namespace Dave6.LootShooter.Samples.Character
{
    public sealed class ProtoTypePlayerBootstrap : MonoBehaviour
    {
        InputRuntime _InputRuntime;

        PrototypeCharacterController _Character;
        ThirdPersonCamera _Camera;

        void Awake()
        {
            _Character = FindAnyObjectByType<PrototypeCharacterController>();
            _Camera = FindAnyObjectByType<ThirdPersonCamera>();

            _InputRuntime = new InputRuntime();

            _Character.Initialize(_InputRuntime.Character, _Camera);
            _Camera.Initialize(_InputRuntime.Character);
        }

        void OnEnable()
        {
            _InputRuntime?.EnableCharacter();
        }
        void OnDisable()
        {
            _InputRuntime?.DisableCharacter();
        }
        void OnDestroy()
        {
            _InputRuntime?.Dispose();
        }
    }
}

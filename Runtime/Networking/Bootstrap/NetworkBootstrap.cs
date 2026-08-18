using System;
using Dave6.LootShooter.Camera;
using Dave6.LootShooter.Input;
using Dave6.LootShooter.Networking.Player;
using Dave6.LootShooter.Networking.Runtime;
using Dave6.LootShooter.Networking.Session;
using Unity.Netcode;
using UnityEngine;

namespace Dave6.LootShooter.Networking.Bootstrap
{
    public class NetworkBootstrap : MonoBehaviour
    {
        NetworkRuntime _NetworkRuntime;
        InputRuntime _InputRuntime;
        public NetworkSessionController Session => _NetworkRuntime.Session;
        public PlayerRuntime Player => _NetworkRuntime.Player;
        public ICharacterInput CharacterInput => _InputRuntime.Character;

        public event Action OnReady;
        public bool IsReady { get; private set; }

        [SerializeField] NetworkObject _PlayerPrefab;
        [SerializeField] ThirdPersonCamera _FollowCamera;

        void Awake()
        {
            Initialize();
        }
        void OnDestroy()
        {
            _NetworkRuntime?.Dispose();
            _InputRuntime?.Dispose();
        }

        void Initialize()
        {
            var networkManager = NetworkManager.Singleton;
            if (networkManager == null)
            {
                Debug.LogError("NetworkManager is not present in the scene. Please add a NetworkManager component.");
                return;
            }

            _InputRuntime = new InputRuntime();
            _NetworkRuntime = new NetworkRuntime(networkManager, _PlayerPrefab, _InputRuntime.Character, _FollowCamera);
            _NetworkRuntime.Player.OnPlayerRegistered += HandlePlayerRegistered;
            OnReady?.Invoke();
            IsReady = true;
        }
        void HandlePlayerRegistered(PlayerNetworkController player)
        {
            if (!player.IsOwner) return;
            _InputRuntime.EnableCharacter();
        }
    }
}
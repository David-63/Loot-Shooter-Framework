using System;
using System.Threading.Tasks;
using Dave6.LootShooter.Camera;
using Dave6.LootShooter.Gameplay.Projectile.Visual;
using Dave6.LootShooter.Input;
using Dave6.LootShooter.Networking.Object;
using Dave6.LootShooter.Networking.Runtime;
using Dave6.LootShooter.Networking.Session;
using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEngine;

namespace Dave6.LootShooter.Networking.Bootstrap
{
    public class NetworkBootstrap : MonoBehaviour
    {
        UnityServiceInitializer _ServiceInitializer; // 지역변수로 사용해도 ㄱㅊ
        NetworkRuntime _NetworkRuntime;
        InputRuntime _InputRuntime;
        public NetworkSessionController Session => _NetworkRuntime.Session;
        public PlayerRuntime Player => _NetworkRuntime.Player;
        public ICharacterInput CharacterInput => _InputRuntime.Character;

        public event Action OnReady;
        public bool IsReady { get; private set; }

        [SerializeField] NetworkObject _PlayerPrefab;
        [SerializeField] ThirdPersonCamera _FollowCamera;
        [SerializeField] ProjectileVfx _ProjectileVfx;
        [SerializeField] ImpactVfx _ImpactVfx;

        // void Awake()
        // {
        //     Initialize();
        // }
        async void Awake()
        {
            await InitializeAsync();
        }
        void OnDestroy()
        {
            _NetworkRuntime?.Dispose();
            _InputRuntime?.Dispose();
        }
        
        async Task InitializeAsync()
        {
            var networkManager = NetworkManager.Singleton;
            if (networkManager == null)
            {
                Debug.LogError("NetworkManager is not present in the scene. " + "Please add a NetworkManager component.");
                return;
            }

            _ServiceInitializer = new();
            await _ServiceInitializer.InitializeAsync();

            Debug.Log($"UGS Player ID: " + $"{AuthenticationService.Instance.PlayerId}");

            _InputRuntime = new InputRuntime();
            _NetworkRuntime = new NetworkRuntime(networkManager, _PlayerPrefab, _InputRuntime.Character, _FollowCamera, _ProjectileVfx, _ImpactVfx);
            _NetworkRuntime.Player.OnPlayerRegistered += HandlePlayerRegistered;
            IsReady = true;
            OnReady?.Invoke();
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
            _NetworkRuntime = new NetworkRuntime(networkManager, _PlayerPrefab, _InputRuntime.Character, _FollowCamera, _ProjectileVfx, _ImpactVfx);
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
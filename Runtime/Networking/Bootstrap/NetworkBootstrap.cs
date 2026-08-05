using System;
using Dave6.LootShooter.Networking.Runtime;
using Dave6.LootShooter.Networking.Session;
using Unity.Netcode;
using UnityEngine;

namespace Dave6.LootShooter.Networking.Bootstrap
{
    public class NetworkBootstrap : MonoBehaviour
    {
        NetworkRuntime _NetworkRuntime;
        public NetworkSessionController Session => _NetworkRuntime.Session;
        public PlayerRuntime Player => _NetworkRuntime.Player;
        public event Action OnReady;

        [SerializeField] NetworkObject _PlayerPrefab;

        public void StartHost() => Session.StartHost();
        public void StartClient() => Session.StartClient();
        public void Shutdown() => Session.Shutdown();
        void Awake()
        {
            Initialize();
        }
        void OnDestroy()
        {
            if (_NetworkRuntime == null) return;
            _NetworkRuntime.Dispose();
        }

        void Initialize()
        {
            var networkManager = NetworkManager.Singleton;
            if (networkManager == null)
            {
                Debug.LogError("NetworkManager is not present in the scene. Please add a NetworkManager component.");
                return;
            }
            networkManager.AddNetworkPrefab(_PlayerPrefab.gameObject);
            _NetworkRuntime = new NetworkRuntime(networkManager, _PlayerPrefab);

            //networkManager.NetworkConfig.PlayerPrefab = _PlayerPrefab.gameObject;
            OnReady?.Invoke();
        }

    }
}
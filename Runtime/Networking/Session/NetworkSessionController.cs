using System;
using Unity.Netcode;
using UnityEngine;

namespace Dave6.LootShooter.Networking.Session
{
    public enum NetworkSessionState
    {
        Offline,
        Starting,
        Hosting,
        ClientConnected
    }
    /// <summary>
    /// NGO 이벤트를 받아서 세션 레벨 이벤트로 변환
    /// </summary>
    public class NetworkSessionController : IDisposable
    {
        NetworkManager _NetworkManager;

        public NetworkSessionState State { get; private set; }
        public event Action<NetworkSessionState> OnStateChanged;
        public event Action OnShutdown;
        public event Action<ulong> OnClientJoined;
        public event Action<ulong> OnClientDisconnected;
        

        public bool IsNetworkAvailable() => _NetworkManager != null;
        public bool IsClient => _NetworkManager.IsClient;
        public bool IsServer => _NetworkManager.IsServer;
        public string GetNetworkStatus()
        {
            if (_NetworkManager.IsHost) return "Host";
            if (_NetworkManager.IsServer) return "Server";
            if (_NetworkManager.IsClient) return "Client";
            return "Not Connected";
        }
        public string GetTransportName()
        {
            if (_NetworkManager == null) return "No NetworkManager";
            return _NetworkManager.NetworkConfig.NetworkTransport.GetType().Name;
        }

        public NetworkSessionController(NetworkManager networkManager)
        {
            _NetworkManager = networkManager;

            _NetworkManager.OnServerStarted += HandleServerStarted;
            _NetworkManager.OnClientConnectedCallback += HandleClientConnected;
            _NetworkManager.OnClientDisconnectCallback += HandleClientDisconnected;
            _NetworkManager.OnClientStopped += HandleClientStopped;
        }
        public void Dispose()
        {
            _NetworkManager.OnServerStarted -= HandleServerStarted;
            _NetworkManager.OnClientConnectedCallback -= HandleClientConnected;
            _NetworkManager.OnClientDisconnectCallback -= HandleClientDisconnected;
            _NetworkManager.OnClientStopped -= HandleClientStopped;
        }

        void ChangeState(NetworkSessionState next)
        {
            State = next;
            OnStateChanged?.Invoke(next);
        }

        void HandleServerStarted()
        {
            ChangeState(NetworkSessionState.Hosting);
        }
        void HandleClientConnected(ulong clientId)
        {
            Debug.Log(
            $"[NetworkSession] ClientConnected | " +
            $"ClientId={clientId} | " +
            $"LocalClientId={_NetworkManager.LocalClientId} | " +
            $"IsServer={_NetworkManager.IsServer} | " +
            $"IsClient={_NetworkManager.IsClient} | " +
            $"IsHost={_NetworkManager.IsHost}");
            OnClientJoined?.Invoke(clientId);
            if (clientId == _NetworkManager.LocalClientId)
            {
                ChangeState(NetworkSessionState.ClientConnected);
            }
        }
        void HandleClientDisconnected(ulong clientId)
        {
            OnClientDisconnected?.Invoke(clientId);
        }
        void HandleClientStopped(bool result)
        {
            ChangeState(NetworkSessionState.Offline);
            OnShutdown?.Invoke();
        }

        public void StartHost()
        {
            ChangeState(NetworkSessionState.Starting);
            _NetworkManager.StartHost();
        }
        public void StartClient()
        {
            ChangeState(NetworkSessionState.Starting);
            _NetworkManager.StartClient();
        }
        public void Shutdown()
        {
            _NetworkManager.Shutdown();
        }
    }

}



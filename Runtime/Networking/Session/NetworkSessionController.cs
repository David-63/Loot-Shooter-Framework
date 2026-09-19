using System;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Services.Multiplayer;
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

        ISession _Session;

        public NetworkSessionState State { get; private set; }
        public event Action<NetworkSessionState> OnStateChanged;
        public event Action OnShutdown;
        public event Action<ulong> OnClientJoined;
        public event Action<ulong> OnClientDisconnected;

        public event Action<string> OnJoinCodeChanged;

        public string JoinCode => _Session?.Code;
        public bool IsNetworkAvailable() => _NetworkManager != null;
        public bool IsClient => _NetworkManager != null && _NetworkManager.IsClient;
        public bool IsServer => _NetworkManager != null && _NetworkManager.IsServer;
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

            _NetworkManager.OnClientConnectedCallback += HandleClientConnected;
            _NetworkManager.OnClientDisconnectCallback += HandleClientDisconnected;
            _NetworkManager.OnClientStopped += HandleClientStopped;
        }
        public void Dispose()
        {
            _NetworkManager.OnClientConnectedCallback -= HandleClientConnected;
            _NetworkManager.OnClientDisconnectCallback -= HandleClientDisconnected;
            _NetworkManager.OnClientStopped -= HandleClientStopped;
        }

        async Task<ISession> CreateSessionInternalAsync()
        {
            var options = new SessionOptions { MaxPlayers = 6 }.WithRelayNetwork();

            return await MultiplayerService.Instance.CreateSessionAsync(options);
        }
        public async Task CreateSessionAsync()
        {
            ChangeState(NetworkSessionState.Starting);

            try
            {
                _Session = await CreateSessionInternalAsync();
                // 여기까지 세션 생성이 성공하면, MPS가 Relay/NGO 연결을 구성함.

                Debug.Log($"[NetworkSession] Created | " + $"Code={_Session.Code}");
                OnJoinCodeChanged?.Invoke(JoinCode);

                ChangeState(NetworkSessionState.Hosting);
            }
            catch (Exception exception)
            {
                ChangeState(NetworkSessionState.Offline);
                Debug.LogException(exception);
            }
        }
        public async Task JoinSessionAsync(string code)
        {
            ChangeState(NetworkSessionState.Starting);

            try
            {
                _Session = await MultiplayerService.Instance.JoinSessionByCodeAsync(code);
                Debug.Log($"[NetworkSession] Joined | " + $"Code={_Session.Code}");
                ChangeState(NetworkSessionState.ClientConnected);
            }
            catch (Exception exception)
            {
                ChangeState(NetworkSessionState.Offline);
                Debug.LogException(exception);
            }
        }
        public async Task LeaveSessionAsync()
        {
            if (_Session == null) return;

            await _Session.LeaveAsync();

            _Session = null;

            ChangeState(NetworkSessionState.Offline);
            OnShutdown?.Invoke();
        }

        void ChangeState(NetworkSessionState next)
        {
            State = next;
            OnStateChanged?.Invoke(next);
        }

        void HandleClientConnected(ulong clientId)
        {
            OnClientJoined?.Invoke(clientId);

            Debug.Log($"[NetworkSession] ClientConnected | " + $"ClientId={clientId}");

            if (_NetworkManager.IsClient && clientId == _NetworkManager.LocalClientId)
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

        // public void StartHost()
        // {
        //     ChangeState(NetworkSessionState.Starting);
        //     _NetworkManager.StartHost();
        // }
        // public void StartClient()
        // {
        //     ChangeState(NetworkSessionState.Starting);
        //     _NetworkManager.StartClient();
        // }
        // public void Shutdown()
        // {
        //     _NetworkManager.Shutdown();
        // }
    }

}



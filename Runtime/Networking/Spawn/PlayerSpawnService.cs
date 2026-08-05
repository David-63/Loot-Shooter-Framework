using System;
using Dave6.LootShooter.Networking.Player;
using Dave6.LootShooter.Networking.Runtime;
using Dave6.LootShooter.Networking.Session;
using Unity.Netcode;
using UnityEngine;

namespace Dave6.LootShooter.Networking.Spawn
{
    /// <summary>
    /// Network Object 생성/제거 담당
    /// </summary>
    public class PlayerSpawnService : IDisposable
    {
        readonly NetworkManager _NetworkManager;
        readonly NetworkObject _PlayerPrefab;
        readonly PlayerRuntime _PlayerRuntime;
        readonly NetworkSessionController _Session;

        public PlayerSpawnService(NetworkManager networkManager, NetworkObject prefab, NetworkSessionController session, PlayerRuntime playerRuntime)
        {
            _NetworkManager = networkManager;
            _PlayerPrefab = prefab;
            _PlayerRuntime = playerRuntime;
            _Session = session;

            _Session.OnClientJoined += SpawnPlayer;
            _Session.OnClientDisconnected += DespawnPlayer;
        }

        public void Dispose()
        {
            _Session.OnClientJoined -= SpawnPlayer;
            _Session.OnClientDisconnected -= DespawnPlayer;
        }

        public void SpawnPlayer(ulong clientId)
        {
            if (!_NetworkManager.IsServer) return;

            var obj = UnityEngine.Object.Instantiate(_PlayerPrefab);
            var player = obj.GetComponent<PlayerNetworkController>(); // 나중에 참조를 위한 Root컴포넌트를 추가할 예정

            player.Spawned += HandlePlayerSpawn;
            player.Despawned += HandlePlayerDespawn;

            obj.SpawnAsPlayerObject(clientId);
        }
        public void DespawnPlayer(ulong clientId)
        {
            if (!_NetworkManager.IsServer) return;
            if (!_PlayerRuntime.Players.TryGetValue(clientId, out var player)) return;

            player.NetworkObject.Despawn(true);
        }

        void HandlePlayerSpawn(PlayerNetworkController player)
        {
            player.Spawned -= HandlePlayerSpawn;
            _PlayerRuntime.Register(player);
        }

        void HandlePlayerDespawn(PlayerNetworkController player)
        {
            player.Despawned -= HandlePlayerDespawn;
            _PlayerRuntime.Unregister(player);
        }
    }
}
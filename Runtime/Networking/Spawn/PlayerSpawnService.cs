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
    /// 
    /// 서버에서 플레이어를 생성하는 책임만 담당
    /// 누가 생성했는지는 Player Runtime 클래스가 수행해야함
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

            Debug.Log(
            $"[PlayerSpawn] SpawnPlayer BEGIN | " +
            $"ClientId={clientId} | " +
            $"IsServer={_NetworkManager.IsServer}");

            var obj = UnityEngine.Object.Instantiate(_PlayerPrefab);

            Debug.Log(
            $"[PlayerSpawn] Instantiate | " +
            $"Object={obj.name} | " +
            $"ClientId={clientId}");

            obj.SpawnAsPlayerObject(clientId);

            Debug.Log(
            $"[PlayerSpawn] SpawnAsPlayerObject COMPLETE | " +
            $"Object={obj.name} | " +
            $"NetworkObjectId={obj.NetworkObjectId} | " +
            $"OwnerClientId={obj.OwnerClientId}");
        }
        public void DespawnPlayer(ulong clientId)
        {
            if (!_NetworkManager.IsServer) return;

            if (!_PlayerRuntime.Players.TryGetValue(clientId, out var player)) return;

            player.NetworkObject.Despawn(true);
        }
    }
}
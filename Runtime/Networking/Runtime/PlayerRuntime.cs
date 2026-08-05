using System;
using System.Collections.Generic;
using Dave6.LootShooter.Networking.Player;
using UnityEngine;

namespace Dave6.LootShooter.Networking.Runtime
{
    /// <summary>
    /// 플레이어 등록, 제거, 조회
    /// </summary>
    public sealed class PlayerRuntime : IDisposable
    {
        readonly Dictionary<ulong, PlayerNetworkController> _Players = new();
        public IReadOnlyDictionary<ulong, PlayerNetworkController> Players => _Players;
        public PlayerNetworkController LocalPlayer { get; private set; }
        public bool TryGetPlayer(ulong id, out PlayerNetworkController player) => _Players.TryGetValue(id, out player);

        public event Action<PlayerNetworkController> OnPlayerRegistered;
        public event Action<PlayerNetworkController> OnPlayerUnregistered;

        public void Dispose()
        {
            _Players.Clear();
            LocalPlayer = null;
        }
        public void Register(PlayerNetworkController player)
        {
            _Players[player.OwnerClientId] = player;

            if (player.IsOwner) LocalPlayer = player;
            
            OnPlayerRegistered?.Invoke(player);
        }

        public void Unregister(PlayerNetworkController player)
        {
            _Players.Remove(player.OwnerClientId);

            if (LocalPlayer == player) LocalPlayer = null;

            OnPlayerUnregistered?.Invoke(player);
        }
    }
}
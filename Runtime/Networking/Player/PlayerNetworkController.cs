using System;
using Unity.Netcode;
using UnityEngine;

namespace Dave6.LootShooter.Networking.Player
{
    /// <summary>
    /// Ownership 판단, Local Component 활성화, Server Authority 연결, Input 전달
    /// Coordinator 역할 수행: onwer 여부에 따라 모든 컴포넌트를 연결
    /// </summary>
    public class PlayerNetworkController : NetworkBehaviour
    {
        public event Action<PlayerNetworkController> Spawned;
        public event Action<PlayerNetworkController> Despawned;
        public override void OnNetworkSpawn()
        {
            Spawned?.Invoke(this);

            if (IsOwner) InitializeLocalPlayer();
        }

        public override void OnNetworkDespawn()
        {
            Despawned?.Invoke(this);
        }

        void InitializeLocalPlayer()
        {
            Debug.Log("Local Player Initialized");
        }
        void InitializeRemotePlayer()
        {
            
        }
        void EnableLocalComponents()
        {
            
        }
        void DisableLocalComponents()
        {
            
        }
    }
}
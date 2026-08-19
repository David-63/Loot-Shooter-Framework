using System;
using Dave6.LootShooter.Networking.Bootstrap;
using Dave6.LootShooter.Networking.Runtime;
using Unity.Netcode;
using UnityEngine;

namespace Dave6.LootShooter.Networking.Player
{
    /// <summary>
    /// Ownership 판단, Local Component 활성화, Server Authority 연결, Input 전달
    /// Coordinator 역할 수행: onwer 여부에 따라 모든 컴포넌트를 연결
    /// 
    /// 사실상 네트워크 생명주기만 전담
    /// </summary>
    public class PlayerNetworkController : NetworkBehaviour
    {
        [field: SerializeField] public Transform FollowTarget { get; private set; }

        public override void OnNetworkSpawn()
        {
            Debug.Log(
            $"[PlayerNetwork] OnNetworkSpawn | " +
            $"GameObject={name} | " +
            $"NetworkObjectId={NetworkObjectId} | " +
            $"OwnerClientId={OwnerClientId} | " +
            $"LocalClientId={NetworkManager.LocalClientId} | " +
            $"IsOwner={IsOwner} | " +
            $"IsServer={IsServer} | " +
            $"IsClient={IsClient} | " +
            $"IsHost={IsHost}");
            var bootstrap = FindAnyObjectByType<NetworkBootstrap>();
            bootstrap.Player.Register(this);
        }

        public override void OnNetworkDespawn()
        {
            var bootstrap = FindAnyObjectByType<NetworkBootstrap>();
            bootstrap.Player.Unregister(this);
        }

        public void SetLocalPlayer()
        {
            gameObject.tag = "LocalPlayer";
        }
    }
}
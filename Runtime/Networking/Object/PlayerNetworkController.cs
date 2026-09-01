using Dave6.LootShooter.Character.Network;
using Dave6.LootShooter.Networking.Bootstrap;
using Dave6.LootShooter.Networking.Spawn;
using Unity.Netcode;
using UnityEngine;

namespace Dave6.LootShooter.Networking.Object
{
    /// <summary>
    /// Ownership 판단, Local Component 활성화, Server Authority 연결, Input 전달
    /// Coordinator 역할 수행: onwer 여부에 따라 모든 컴포넌트를 연결
    /// 
    /// 사실상 네트워크 생명주기만 전담
    /// </summary>
    public class PlayerNetworkController : NetworkBehaviour, ICharacterNetwork
    {
        [field: SerializeField] public Transform FollowTarget { get; private set; }
        ProjectileSpawnService _SpawnProjectile;

        public void Initialize(ProjectileSpawnService spawnProjectile) => _SpawnProjectile = spawnProjectile;

        public override void OnNetworkSpawn()
        {
            var bootstrap = FindAnyObjectByType<NetworkBootstrap>();
            bootstrap.Player.Register(this);
        }

        public override void OnNetworkDespawn()
        {
            var bootstrap = FindAnyObjectByType<NetworkBootstrap>();
            bootstrap.Player.Unregister(this);
        }

        /// <summary>
        /// 카메라 충돌 방지
        /// </summary>
        public void SetLocalPlayer()
        {
            gameObject.tag = "LocalPlayer";
        }
        public void RequestFire()
        {
            FireServerRpc();
        }

        [ServerRpc]
        void FireServerRpc()
        {
            _SpawnProjectile.Spawn(transform.position + transform.forward, transform.rotation);
        }
    }
}
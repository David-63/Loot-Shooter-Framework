using Unity.Netcode;
using UnityEngine;

namespace Dave6.LootShooter.Networking.Spawn
{
    public sealed class ProjectileSpawnService
    {
        readonly NetworkObject _ProjectilePrefab;
        public ProjectileSpawnService(NetworkObject projectilePrefab) => _ProjectilePrefab = projectilePrefab;

        public void Spawn(Vector3 position, Quaternion rotation)
        {
            var projectile = UnityEngine.Object.Instantiate(_ProjectilePrefab, position, rotation);
            projectile.Spawn();
        }
    }
}
using System;
using Dave6.LootShooter.Camera;
using Dave6.LootShooter.Input;
using Dave6.LootShooter.Networking.Session;
using Dave6.LootShooter.Networking.Spawn;
using Unity.Netcode;

namespace Dave6.LootShooter.Networking.Runtime
{
    public sealed class NetworkRuntime : IDisposable
    {
        public NetworkSessionController Session { get; }
        public PlayerRuntime Player { get; }
        public PlayerSpawnService SpawnPlayer { get; }
        public ProjectileSpawnService SpawnProjectile { get; }
        public NetworkRuntime(NetworkManager manager, NetworkObject player, NetworkObject projectile, ICharacterInput input, ThirdPersonCamera camera)
        {
            Session = new NetworkSessionController(manager);
            SpawnProjectile = new ProjectileSpawnService(projectile);

            Player = new PlayerRuntime(input, camera, SpawnProjectile);
            SpawnPlayer = new PlayerSpawnService(manager, player, Session, Player);
        }

        public void Dispose()
        {
            SpawnPlayer.Dispose();
            Player.Dispose();
            Session.Dispose();
        }
    }
}
using System;
using Dave6.LootShooter.Camera;
using Dave6.LootShooter.Gameplay.Projectile;
using Dave6.LootShooter.Gameplay.Projectile.Visual;
using Dave6.LootShooter.Input;
using Dave6.LootShooter.Networking.Session;
using Dave6.LootShooter.Networking.Spawn;
using Unity.Netcode;

namespace Dave6.LootShooter.Networking.Runtime
{
    public sealed class NetworkRuntime : IDisposable
    {
        NetworkManager _NetManager;
        public NetworkSessionController Session { get; }
        public PlayerRuntime Player { get; }
        public PlayerSpawnService SpawnPlayer { get; }
        public ProjectileSimulationManager Projectiles { get; } // gameplay runtime 을 추가한다면 거기로 옮기기
        public ProjectileVfxManager Vfxes { get; } // gameplay runtime 을 추가한다면 거기로 옮기기
        public NetworkRuntime(NetworkManager manager, NetworkObject player, ICharacterInput input, ThirdPersonCamera camera, ProjectileVfx projectile, ImpactVfx impact)
        {
            _NetManager = manager;
            Session = new NetworkSessionController(manager);
            Projectiles = new ProjectileSimulationManager();
            Vfxes = new ProjectileVfxManager(projectile, impact);

            Player = new PlayerRuntime(input, camera, Projectiles, Vfxes);
            SpawnPlayer = new PlayerSpawnService(manager, player, Session, Player);

            _NetManager.OnServerStarted += OnServerStarted;
        }

        public void Dispose()
        {
            _NetManager.OnServerStarted -= OnServerStarted;
            if (_NetManager.NetworkTickSystem != null) _NetManager.NetworkTickSystem.Tick -= OnNetworkTick;
            SpawnPlayer.Dispose();
            Player.Dispose();
            Session.Dispose();
        }

        void OnServerStarted()
        {
            _NetManager.NetworkTickSystem.Tick += OnNetworkTick;
        }

        void OnNetworkTick()
        {
            if (!_NetManager.IsServer) return;

            Projectiles.Simulate(1f / _NetManager.NetworkConfig.TickRate);
        }
    }
}
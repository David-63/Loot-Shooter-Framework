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
        public PlayerSpawnService Spawn { get; }
        public NetworkRuntime(NetworkManager manager, NetworkObject prefab, ICharacterInput input, ThirdPersonCamera camera)
        {
            Session = new NetworkSessionController(manager);
            Player = new PlayerRuntime(input, camera);
            Spawn = new PlayerSpawnService(manager, prefab, Session, Player);
        }

        public void Dispose()
        {
            Spawn.Dispose();
            Player.Dispose();
            Session.Dispose();
        }
    }
}
using System;
using System.Collections.Generic;
using Dave6.LootShooter.Camera;
using Dave6.LootShooter.Character;
using Dave6.LootShooter.Gameplay.Projectile;
using Dave6.LootShooter.Input;
using Dave6.LootShooter.Networking.Object;
using Dave6.LootShooter.Networking.Spawn;
using UnityEngine;

namespace Dave6.LootShooter.Networking.Runtime
{
    /// <summary>
    /// 플레이어 등록, 제거, 조회
    /// </summary>
    public sealed class PlayerRuntime : IDisposable
    {
        ICharacterInput _CharacterInput;
        ThirdPersonCamera _LocalCamera;
        ProjectileSimulationManager _SimulationManager;
        ProjectileVfxManager _VisualManager;

        readonly Dictionary<ulong, PlayerNetworkController> _Players = new();
        public IReadOnlyDictionary<ulong, PlayerNetworkController> Players => _Players;

        public PlayerNetworkController LocalPlayer { get; private set; }

        public bool TryGetPlayer(ulong id, out PlayerNetworkController player) => _Players.TryGetValue(id, out player);

        public event Action<PlayerNetworkController> OnPlayerRegistered;
        public event Action<PlayerNetworkController> OnPlayerUnregistered;

        public PlayerRuntime(ICharacterInput input, ThirdPersonCamera camera, ProjectileSimulationManager simulation, ProjectileVfxManager visual)
        {
            _CharacterInput = input;
            _LocalCamera = camera;
            _SimulationManager = simulation;
            _VisualManager = visual;
        }

        public void Dispose()
        {
            if (LocalPlayer != null) UnbindLocalPlayer();
            _Players.Clear();
        }
        public void Register(PlayerNetworkController player)
        {
            if (_Players.ContainsKey(player.OwnerClientId))
            {
                Debug.LogWarning(
                    $"[PlayerRuntime] Register SKIP - Already Registered | " +
                    $"Owner={player.OwnerClientId}");

                return;
            }
            _Players[player.OwnerClientId] = player;

            player.Initialize(_SimulationManager, _VisualManager);

            if (player.IsOwner)
            {
                Debug.Log(
                    $"[PlayerRuntime] Local Player Detected | " +
                    $"Player={player.name}");

                BindLocalPlayer(player);
            }
            
            OnPlayerRegistered?.Invoke(player);
        }

        public void Unregister(PlayerNetworkController player)
        {
            if (!_Players.Remove(player.OwnerClientId))
            {
                Debug.LogWarning(
                    $"[PlayerRuntime] Unregister SKIP - Not Registered | " +
                    $"Owner={player.OwnerClientId}");

                return;
            }

            if (LocalPlayer == player)
            {
                Debug.Log(
                    $"[PlayerRuntime] Local Player Unbinding | " +
                    $"Player={player.name}");

                UnbindLocalPlayer();
            }

            OnPlayerUnregistered?.Invoke(player);
        }

        public void BindLocalPlayer(PlayerNetworkController player)
        {
            var character = player.GetComponent<CharacterAgent>();
            if (character == null)
            {
                Debug.LogError("Can not find CharacterController.");
                return;
            }
            if (_LocalCamera == null)
            {
                Debug.LogError("Camera is not assigned.");
                return;
            }
            character.Initialize(_CharacterInput, _LocalCamera, player);
            _LocalCamera.Initialize(_CharacterInput);
            _LocalCamera.SetCameraTarget(player.FollowTarget);
            player.SetLocalPlayer();

            LocalPlayer = player;
        }
        public void UnbindLocalPlayer()
        {
            LocalPlayer = null;
            _LocalCamera.SetCameraTarget(null);
        }
    }
}
using System;
using System.Collections.Generic;
using Dave6.LootShooter.Camera;
using Dave6.LootShooter.Input;
using Dave6.LootShooter.Networking.Player;
using Dave6.LootShooter.Networking.Session;
using Dave6.LootShooter.Samples.Character;
using Unity.Netcode;
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

        readonly Dictionary<ulong, PlayerNetworkController> _Players = new();
        public IReadOnlyDictionary<ulong, PlayerNetworkController> Players => _Players;

        public PlayerNetworkController LocalPlayer { get; private set; }

        public bool TryGetPlayer(ulong id, out PlayerNetworkController player) => _Players.TryGetValue(id, out player);

        public event Action<PlayerNetworkController> OnPlayerRegistered;
        public event Action<PlayerNetworkController> OnPlayerUnregistered;

        public PlayerRuntime(ICharacterInput input, ThirdPersonCamera camera)
        {
            _CharacterInput = input;
            _LocalCamera = camera;
        }

        public void Dispose()
        {
            if (LocalPlayer != null) UnbindLocalPlayer();
            _Players.Clear();
        }
        public void Register(PlayerNetworkController player)
        {
            Debug.Log(
            $"[PlayerRuntime] Register BEGIN | " +
            $"Player={player.name} | " +
            $"Owner={player.OwnerClientId} | " +
            $"Local={player.NetworkManager.LocalClientId} | " +
            $"IsOwner={player.IsOwner}");
            if (_Players.ContainsKey(player.OwnerClientId))
            {
                Debug.LogWarning(
                    $"[PlayerRuntime] Register SKIP - Already Registered | " +
                    $"Owner={player.OwnerClientId}");

                return;
            }
            _Players[player.OwnerClientId] = player;

            Debug.Log(
            $"[PlayerRuntime] Registered | " +
            $"Player={player.name} | " +
            $"Owner={player.OwnerClientId} | " +
            $"PlayerCount={_Players.Count}");

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
            Debug.Log(
            $"[PlayerRuntime] Unregister BEGIN | " +
            $"Player={player.name} | " +
            $"Owner={player.OwnerClientId}");
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

            Debug.Log(
            $"[PlayerRuntime] Unregistered | " +
            $"Player={player.name} | " +
            $"RemainingPlayers={_Players.Count}");

            OnPlayerUnregistered?.Invoke(player);
        }

        public void BindLocalPlayer(PlayerNetworkController player)
        {
            Debug.Log(
            $"[PlayerRuntime] BindLocalPlayer BEGIN | " +
            $"Player={player.name} | " +
            $"Owner={player.OwnerClientId}");
            var character = player.GetComponent<PrototypeCharacterController>();
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

            character.Initialize(_CharacterInput, _LocalCamera);
            Debug.Log(
            $"[PlayerRuntime] Character Initialized | " +
            $"Player={player.name}");
            _LocalCamera.Initialize(_CharacterInput);
            _LocalCamera.SetCameraTarget(player.FollowTarget);

            LocalPlayer = player;

            Debug.Log(
            $"[PlayerRuntime] BindLocalPlayer COMPLETE | " +
            $"LocalPlayer={LocalPlayer.name} | " +
            $"CameraTarget={_LocalCamera.Target.name}");
        }
        public void UnbindLocalPlayer()
        {
            LocalPlayer = null;
            _LocalCamera.SetCameraTarget(null);
        }
    }
}
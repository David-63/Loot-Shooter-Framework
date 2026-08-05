
using Dave6.LootShooter.Networking.Bootstrap;
using Dave6.LootShooter.Networking.Player;
using Dave6.LootShooter.Networking.Runtime;
using Unity.Netcode;
using UnityEngine;

namespace Dave6.LootShooter.Samples.Networking.Observer
{
    public class PlayerObserver : MonoBehaviour
    {
        [SerializeField] NetworkBootstrap _Bootstrap;
        PlayerRuntime _PlayerRuntime;

        void OnEnable()
        {
            _Bootstrap.OnReady += Bind;
        }
        void OnDisable()
        {
            _Bootstrap.OnReady -= Bind;
            if (_PlayerRuntime != null)
            {
                _PlayerRuntime.OnPlayerRegistered -= HandleRegister;
                _PlayerRuntime.OnPlayerUnregistered -= HandleUnregister;
            }
        }
        void Bind()
        {
            _PlayerRuntime = _Bootstrap.Player;
            _PlayerRuntime.OnPlayerRegistered += HandleRegister;
            _PlayerRuntime.OnPlayerUnregistered += HandleUnregister;
        }
        void HandleRegister(PlayerNetworkController player)
        {
            Debug.Log($"Player Registered : {player.OwnerClientId}");
        }
        void HandleUnregister(PlayerNetworkController player)
        {
            Debug.Log($"Player Unregistered : {player.OwnerClientId}");
        }
    }
}
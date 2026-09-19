using Dave6.LootShooter.Character;
using Dave6.LootShooter.Character.Network;
using Dave6.LootShooter.Gameplay.Projectile;
using Dave6.LootShooter.Networking.Bootstrap;
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
        CharacterAgent _Player;
        PlayerNetworkState _State;

        PlayerInputData _LastestInput;

        ProjectileSimulationManager _ProjectileSimulation;
        ProjectileVfxManager _ProjectileVisual;

        #region Initialization
        public void Initialize(ProjectileSimulationManager projectileSimulation, ProjectileVfxManager projectileVisual)
        {
            _ProjectileSimulation = projectileSimulation;
            _ProjectileVisual = projectileVisual;
            _ProjectileSimulation.OnProjectileImpact += SpawnImpactVfxClientRpc;
        }

        public override void OnNetworkSpawn()
        {
            var bootstrap = FindAnyObjectByType<NetworkBootstrap>();
            _Player = GetComponent<CharacterAgent>();
            _State = GetComponent<PlayerNetworkState>();
            bootstrap.Player.Register(this);

            _State.IsCrouched.OnValueChanged += OnCrouchedChanged;
            RegisterNetworkTick();
        }

        public override void OnNetworkDespawn()
        {
            UnRegisterNetworkTick();

            _State.IsCrouched.OnValueChanged -= OnCrouchedChanged;

            if (_ProjectileSimulation != null)
            {
                _ProjectileSimulation.OnProjectileImpact -= SpawnImpactVfxClientRpc;
            }
            var bootstrap = FindAnyObjectByType<NetworkBootstrap>();
            bootstrap.Player.Unregister(this);
        }
        #endregion


        #region Network Tick
        void RegisterNetworkTick()
        {
            if (IsOwner)
            {
                NetworkManager.NetworkTickSystem.Tick += HandleNetworkTick;
            }
            if (IsServer)
            {
                NetworkManager.NetworkTickSystem.Tick += HandleServerNetworkTick;
            }
        }

        void UnRegisterNetworkTick()
        {
            if (IsOwner)
            {
                NetworkManager.NetworkTickSystem.Tick -= HandleNetworkTick;
            }
            if (IsServer)
            {
                NetworkManager.NetworkTickSystem.Tick -= HandleServerNetworkTick;
            }
        }

        void HandleNetworkTick()
        {
            var input = _Player.CreateInputData();
            RequestInputServerRpc(new NetworkInputData(input));
        }
        void HandleServerNetworkTick()
        {
            float tickDeltaTime = 1f / NetworkManager.NetworkConfig.TickRate;
            _Player.OnNetworkTick(_LastestInput, tickDeltaTime);

            // player의 crouch 상태를 동기화
            SyncNetworkState();
        }
        #endregion

        #region Input
        [ServerRpc]
        public void RequestInputServerRpc(NetworkInputData input)
        {
            _LastestInput = input.ToInputData();
        }
        #endregion
        #region Network State
        void SyncNetworkState()
        {
            SyncCrouchState();
        }

        void SyncCrouchState()
        {
            bool crouched = _Player.Movement.IsCrouched;
            if (_State.IsCrouched.Value != crouched)
            {
                _State.IsCrouched.Value = crouched;
            }
        }
        void OnCrouchedChanged(bool previous, bool current)
        {
            // Crouched 값 직접 변경하기 (클라이언트 환경)
            if (IsServer) return;

            _Player.Movement.ApplyCrouch(current);
        }
        #endregion

        /// <summary>
        /// 카메라 충돌 방지
        /// </summary>
        public void SetLocalPlayer()
        {
            gameObject.tag = "LocalPlayer";
        }

        #region Combat
        public void RequestFire()
        {
            FireServerRpc();
        }

        [ServerRpc]
        void FireServerRpc()
        {
            var position = transform.position + transform.forward;
            var velocity = transform.forward * 10f; // <- 카메라 값으로 해야하는데..? 일단 보류

            var projectile = new ProjectileSimulation(
                new ProjectileContext{Position = position, Velocity = velocity},
                new IProjectileAbility[]{new MoveAbility(), new HitAbility(), new ImpactAbility()});

            var id = _ProjectileSimulation.AddProjectile(projectile);

            SpawnProjectileVfxClientRpc(id, position, velocity);
        }
        #endregion
        #region Projectile Vfx
        [ClientRpc]
        void SpawnProjectileVfxClientRpc(int id, Vector3 position, Vector3 velocity)
        {
            _ProjectileVisual.SpawnProjectile(id, position, velocity);
        }
        [ClientRpc]
        void SpawnImpactVfxClientRpc(int id, Vector3 position, Vector3 normal)
        {
            _ProjectileVisual.RemoveProjectile(id);
            _ProjectileVisual.SpawnImpactVfx(position, normal);
        }
        #endregion

        void OnDrawGizmos()
        {
            if (_ProjectileSimulation == null) return;

            foreach (var projectile in _ProjectileSimulation.Projectiles)
            {
                Gizmos.DrawSphere(projectile.Context.Position, 0.1f);
            }
        }
    }
}
using Unity.Netcode;

namespace Dave6.LootShooter.Networking.Object
{
    public sealed class PlayerNetworkState : NetworkBehaviour
    {
        public NetworkVariable<bool> IsCrouched { get; } = new();
        public NetworkVariable<bool> IsAiming { get; } = new();
        public NetworkVariable<int> EquippedWeaponId { get; } = new();
    }
}
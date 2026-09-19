using UnityEngine;

namespace Dave6.LootShooter.Gameplay.Projectile
{
    public sealed class HitTarget
    {
        public Collider collider;
        public Vector3 point;
        public Vector3 normal;
    }
    public sealed class ProjectileContext
    {
        public int Id;

        public Vector3 PreviousPosition;
        public Vector3 Position;
        public Vector3 Velocity;

        public HitTarget Hit;

        public bool Consume;
    }
}
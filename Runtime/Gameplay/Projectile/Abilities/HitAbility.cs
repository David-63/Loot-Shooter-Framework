using UnityEngine;

namespace Dave6.LootShooter.Gameplay.Projectile
{
    public sealed class HitAbility : IProjectileAbility
    {
        public void OnUpdate(ProjectileSimulation projectile, float deltaTime)
        {
            var ctx = projectile.Context;

            var direction = ctx.Position - ctx.PreviousPosition;

            var distance = direction.magnitude;

            if (distance <= 0f) return;
            if (!Physics.Raycast(ctx.PreviousPosition, direction.normalized, out var hit, distance)) return;

            ctx.Hit = new HitTarget{ collider = hit.collider, point = hit.point, normal = hit.normal };
        }
    }

}
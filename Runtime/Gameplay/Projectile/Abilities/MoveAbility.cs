using UnityEngine;

namespace Dave6.LootShooter.Gameplay.Projectile
{
    public sealed class MoveAbility : IProjectileAbility
    {
        public void OnUpdate(ProjectileSimulation projectile, float deltaTime)
        {
            projectile.Context.PreviousPosition = projectile.Context.Position;
            projectile.Context.Position += projectile.Context.Velocity * deltaTime;
        }
    }
    public sealed class ImpactAbility : IProjectileAbility
    {
        public void OnUpdate(ProjectileSimulation projectile, float deltaTime)
        {
            var hit = projectile.Context.Hit;
            if (hit == null) return;

            projectile.Context.Consume = true;
        }
    }

}
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dave6.LootShooter.Gameplay.Projectile
{
    public sealed class ProjectileSimulationManager
    {
        readonly List<ProjectileSimulation> _Projectiles = new();
        public IReadOnlyList<ProjectileSimulation> Projectiles => _Projectiles;
        readonly List<ProjectileSimulation> _RemoveBuffer = new();

        int _CountId;

        public event Action<int, Vector3, Vector3> OnProjectileImpact;

        public int AddProjectile(ProjectileSimulation projectile)
        {
            var id = _CountId++;
            projectile.Context.Id = id;
            _Projectiles.Add(projectile);
            return id;
        }

        public void Simulate(float deltaTime)
        {
            foreach (var projectile in _RemoveBuffer)
            {
                _Projectiles.Remove(projectile);
            }
            _RemoveBuffer.Clear();

            foreach (var projectile in _Projectiles)
            {
                projectile.Simulate(deltaTime);

                var hit = projectile.Context.Hit;

                if (hit != null)
                {
                    OnProjectileImpact?.Invoke(projectile.Context.Id, hit.point, hit.normal);
                }

                if (projectile.Context.Consume) _RemoveBuffer.Add(projectile);
            }
        }
    }
}
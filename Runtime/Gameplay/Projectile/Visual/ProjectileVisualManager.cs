using System.Collections.Generic;
using Dave6.LootShooter.Gameplay.Projectile.Visual;
using UnityEngine;

namespace Dave6.LootShooter.Gameplay.Projectile
{
    public sealed class ProjectileVfxManager
    {
        readonly ProjectileVfx _ProjectileVfx;
        readonly ImpactVfx _ImpactVfx;

        //readonly Transform _Root;
        readonly List<ProjectileVfx> _Projectiles = new();
        //readonly List<ImpactVfx> _ImpactVfxs = new();

        public ProjectileVfxManager(ProjectileVfx projectile, ImpactVfx impact)
        {
            _ProjectileVfx = projectile;
            _ImpactVfx = impact;
        }
        public ProjectileVfx SpawnProjectile(int id, Vector3 position, Vector3 velocity)
        {
            var visual = Object.Instantiate(_ProjectileVfx, position, Quaternion.LookRotation(velocity));
            visual.Initialize(id);
            _Projectiles.Add(visual);
            visual.Play();
            return visual;
        }
        public void SpawnImpactVfx(Vector3 position, Vector3 normal)
        {
            var vfx = Object.Instantiate(_ImpactVfx, position, Quaternion.LookRotation(normal));
            vfx.Play();
        }

        public void RemoveProjectile(int id)
        {
            var projectile = _Projectiles.Find(x => x.Id == id);

            if (projectile == null) return;
            _Projectiles.Remove(projectile);

            Object.Destroy(projectile.gameObject);
        }

        public void RemoveProjectile(ProjectileVfx visual)
        {
            if (!_Projectiles.Remove(visual)) return;

            Object.Destroy(visual.gameObject);
        }
    }
}
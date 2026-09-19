using System.Collections.Generic;

namespace Dave6.LootShooter.Gameplay.Projectile
{
    public sealed class ProjectileSimulation
    {
        readonly ProjectileContext _Context;
        public ProjectileContext Context => _Context;
        readonly List<IProjectileAbility> _Abilities;

        public ProjectileSimulation(ProjectileContext context, IEnumerable<IProjectileAbility> abilities)
        {
            _Context = context;
            _Abilities = new List<IProjectileAbility>(abilities);
        }
        public void Simulate(float deltaTime)
        {
            foreach (var ability in _Abilities)
            {
                ability.OnUpdate(this, deltaTime);
            }
        }
    }
}
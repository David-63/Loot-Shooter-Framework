namespace Dave6.LootShooter.Gameplay.Projectile
{
    public interface IProjectileAbility
    {
        void OnUpdate(ProjectileSimulation projectile, float deltaTime);
    }
}
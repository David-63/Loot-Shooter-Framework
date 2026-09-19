using UnityEngine;

namespace Dave6.LootShooter.Gameplay.Projectile.Visual
{
    public sealed class ProjectileVfx : MonoBehaviour
    {
        ParticleSystem _ProjectileParticle;
        public int Id { get; private set; }

        void Awake()
        {
            _ProjectileParticle = GetComponent<ParticleSystem>();
        }
        public void Initialize(int id) => Id = id;
        public void Play()
        {
            _ProjectileParticle.Play();
        }
    }
}
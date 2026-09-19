using UnityEngine;

namespace Dave6.LootShooter.Gameplay.Projectile.Visual
{
    public sealed class ImpactVfx : MonoBehaviour
    {
        ParticleSystem _ImpactParticle;
        void Awake()
        {
            _ImpactParticle = GetComponent<ParticleSystem>();
        }
        public void Play()
        {
            _ImpactParticle.Play();
        }
    }
}
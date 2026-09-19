using Dave6.LootShooter.Foundation.State;
using UnityEngine;

namespace Dave6.LootShooter.Character.States
{
    public sealed class FireState : BaseState<CharacterAgent>
    {
        const float _FireDelay = 0.2f;
        float _Timer;
        public FireState(CharacterAgent controller) : base(controller) {}
        public override void OnEnter()
        {
            _Timer = 0f;
        }
        public override void Update()
        {
            _Timer += Time.deltaTime;
            if (_Timer < _FireDelay) return;

            Fire();
            _Timer = 0f;
        }
        void Fire()
        {
            _Controller.Network.RequestFire();
        }
    }
}
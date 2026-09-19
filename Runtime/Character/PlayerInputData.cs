using UnityEngine;

namespace Dave6.LootShooter.Character
{
    public struct PlayerInputData
    {
        public Vector2 Move;
        public Vector2 Look;
        public bool Jump;
        public bool Sprint;
        public bool Crouch;

        public bool Interact;

        public bool Fire;
        public bool Aim;
        public bool Reload;

        public Vector3 CameraForward;
        public Vector3 CameraRight;
    }
    
}
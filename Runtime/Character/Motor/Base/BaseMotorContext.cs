using UnityEngine;

namespace Dave6.LootShooter.Character.Motor
{
    /// <summary>
    /// 현재 상태
    /// </summary>
    public class BaseMotorContext
    {
        // Ground
        public bool IsGrounded;
        public bool IsCrouched;

        public float BaseSpeed;
        public float TargetSpeed;
        public float ImpulseSpeed;

        public float HorizontalSpeed;
        public float VerticalSpeed;

        // Movement
        public Vector3 BaseMoveDirection;
        public Vector3 TargetMoveDirection;
        public Vector3 Velocity;


        // Rotation
        public float CurrentYaw;
        public float TargetYaw;
    }
}

using System;
using UnityEngine;

namespace Dave6.LootShooter.Character.Motor
{
    /// <summary>
    /// 설정 값
    /// </summary>
    [Serializable]
    public class BaseMotorConfig
    {
        [Header("Movement")]
        public float MaxMoveSpeed = 6.5f;
        public float CrouchSpeedFactor = 0.6f;
        public float GroundSpeedChangeRate = 10f;
        public float AirSpeedChangeRate = 0.5f;

        [Header("Gravity")]
        public float Gravity = -15f;
        public float GroundGravity = -4f;
        public float TerminalVelocity = -53f;

        [Header("Jump")]
        public float JumpHeight = 2f;
        public float JumpCooldown = 0.1f;

        [Header("Character")]
        public float Height = 1.6f;
        public float Radius = 0.28f;
        public float StepHeightRatio = 0.14f;
        public Vector3 Center = new(0f, 0.8f, 0f);
    }
}

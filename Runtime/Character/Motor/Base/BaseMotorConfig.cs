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
        public float MaxMoveSpeed = 5.5f;
        public float SpeedChangeRate = 10f; // 나중에 감속 가속 분리할 예정

        [Header("Gravity")]
        public float Gravity = -15f;
        public float GroundGravity = -4f;
        public float TerminalVelocity = -53f;

        [Header("Jump")]
        public float JumpHeight = 2f;
        public float JumpCooldown = 0.1f;

        [Header("Character")]
        public float Height = 1.8f;
        public float Radius = 0.28f;
        public float StepHeightRatio = 0.14f;
        public Vector3 Center = new(0f, 0.5f, 0f);
    }
}

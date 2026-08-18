using UnityEngine;

namespace Dave6.LootShooter.Character.Motor
{
    /// <summary>
    /// 순수 계산
    /// </summary>
    public class BaseMotorAction
    {
        readonly BaseMotorConfig _Config;

        public BaseMotorAction(BaseMotorConfig config) => _Config = config;

        public void UpdateGravity(BaseMotorContext context, float deltaTime)
        {
            if (context.IsGrounded && context.VerticalSpeed < 0f)
            {
                context.VerticalSpeed = _Config.GroundGravity;
            }

            context.VerticalSpeed += _Config.Gravity * deltaTime;

            context.VerticalSpeed = Mathf.Max(context.VerticalSpeed, _Config.TerminalVelocity);
        }

        public void UpdateSpeed(BaseMotorContext context, float deltaTime)
        {
            context.HorizontalSpeed = Mathf.MoveTowards(
                context.HorizontalSpeed, context.TargetSpeed, _Config.SpeedChangeRate * deltaTime);

            //context.ImpulseSpeed = Mathf.MoveTowards(context.ImpulseSpeed, 0f, _Config.SpeedChangeRate * deltaTime);
        }

        public void UpdateVelocity(BaseMotorContext context)
        {
            context.Velocity = context.MoveDirection * context.HorizontalSpeed + Vector3.up * context.VerticalSpeed;
        }

        public void Jump(BaseMotorContext context)
        {
            context.VerticalSpeed = Mathf.Sqrt(_Config.JumpHeight * -2f * _Config.Gravity);
        }
    }
}

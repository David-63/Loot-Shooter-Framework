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

        public void UpdateGrounded(BaseMotor motor, CharacterController controller, BaseMotorContext context)
        {
            motor.GroundSensor.SetOrigin(controller.bounds.center);
            motor.GroundSensor.Length = controller.height * 0.5f - controller.radius * 0.05f;
            context.IsGrounded = motor.GroundSensor.Cast();
        }

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
        public void UpdateRotation(CharacterController controller, BaseMotorContext context, float deltaTime)
        {
            float rotationSpeed = 360f;

            context.CurrentYaw = Mathf.MoveTowardsAngle(controller.transform.eulerAngles.y, context.TargetYaw, rotationSpeed * deltaTime);
            controller.transform.rotation = Quaternion.Euler(0f, context.CurrentYaw, 0f);
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

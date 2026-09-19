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
            // context.HorizontalSpeed = Mathf.MoveTowards(
            //     context.HorizontalSpeed, context.TargetSpeed, _Config.GroundSpeedChangeRate * deltaTime);
            float speedOffset = 0.1f;

            float changeRate = context.IsGrounded ? _Config.GroundSpeedChangeRate : _Config.AirSpeedChangeRate;
            context.BaseSpeed = Mathf.Lerp(context.BaseSpeed, context.TargetSpeed, deltaTime * changeRate);
            context.BaseSpeed = Mathf.Round(context.BaseSpeed * 1000f) / 1000f;

            if (Mathf.Abs(context.BaseSpeed - context.TargetSpeed) <= speedOffset)
            {
                context.BaseSpeed = context.TargetSpeed;
            }

            // 공중 감속 로직 (아직 적용 X)
            // {
            //     float airPenalty = 0.5f;
            //     context.BaseSpeed = Mathf.Lerp(context.BaseSpeed, 0, deltaTime * airPenalty);
            //     context.BaseSpeed = Mathf.Round(context.BaseSpeed * 1000f) / 1000f;
            // }

            if (Mathf.Abs(context.ImpulseSpeed) > speedOffset)
            {
                context.ImpulseSpeed = Mathf.Lerp(context.ImpulseSpeed, 0, deltaTime * _Config.GroundSpeedChangeRate);
                context.ImpulseSpeed = Mathf.Round(context.ImpulseSpeed * 1000f) / 1000f;
            }
            else
            {
                context.ImpulseSpeed = 0;
            }

            var totalSpeed = context.BaseSpeed * (context.IsCrouched ? _Config.CrouchSpeedFactor : 1f);

            context.HorizontalSpeed = totalSpeed + context.ImpulseSpeed;
        }

        public void UpdateRotation(CharacterController controller, BaseMotorContext context, float deltaTime)
        {
            float rotationSpeed = 360f;

            context.CurrentYaw = Mathf.MoveTowardsAngle(controller.transform.eulerAngles.y, context.TargetYaw, rotationSpeed * deltaTime);
            controller.transform.rotation = Quaternion.Euler(0f, context.CurrentYaw, 0f);
        }

        public void UpdateMoveDirection(BaseMotorContext context, float deltaTime)
        {
            if (context.IsGrounded)
            {
                context.BaseMoveDirection = context.TargetMoveDirection;
            }
            else
            {
                context.BaseMoveDirection = Vector3.RotateTowards(
                    context.BaseMoveDirection, context.TargetMoveDirection, _Config.GroundSpeedChangeRate * Mathf.Deg2Rad * deltaTime, 0f);
            }
        }


        public void UpdateVelocity(BaseMotorContext context)
        {
            context.Velocity = context.BaseMoveDirection * context.HorizontalSpeed + Vector3.up * context.VerticalSpeed;
        }

        public void Jump(BaseMotorContext context)
        {
            context.VerticalSpeed = Mathf.Sqrt(_Config.JumpHeight * -2f * _Config.Gravity);
        }
    }
}

using UnityEngine;

namespace Dave6.LootShooter.Foundation.Sensor
{
    public static class PhysicsCaster
    {
        public static bool Cast(Vector3 origin, Vector3 direction, CastConfig config, out RaycastHit hit)
        {
            direction.Normalize();
            if (config.Radius > 0f)
            {
                return Physics.SphereCast(origin, config.Radius, direction, out hit, config.Length, config.LayerMask, config.TriggerInteraction);
            }
            return Physics.Raycast(origin, direction, out hit, config.Length, config.LayerMask, config.TriggerInteraction);
        }
    }
}

using UnityEngine;

namespace Dave6.LootShooter.Foundation.Sensor
{
    public sealed class CastConfig
    {
        public float Length = 1f;
        public float Radius = 0f;
        public LayerMask LayerMask = ~0;
        public QueryTriggerInteraction TriggerInteraction = QueryTriggerInteraction.Ignore;
    }
}

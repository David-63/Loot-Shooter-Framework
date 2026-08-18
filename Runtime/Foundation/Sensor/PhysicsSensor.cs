using UnityEngine;

namespace Dave6.LootShooter.Foundation.Sensor
{
    public sealed class PhysicsSensor
    {
        readonly Transform _Root;
        readonly CastConfig _Config = new();

        Vector3 _LocalOrigin;
        Vector3 _Direction = Vector3.forward;

        RaycastHit _Hit;

        public PhysicsSensor(Transform root) => _Root = root;
        public float Length
        {
            get => _Config.Length;
            set => _Config.Length = value;
        }
        public float Radius
        {
            get => _Config.Radius;
            set => _Config.Radius = value;
        }

        public LayerMask LayerMask
        {
            get => _Config.LayerMask;
            set => _Config.LayerMask = value;
        }

        public QueryTriggerInteraction TriggerInteraction
        {
            get => _Config.TriggerInteraction;
            set => _Config.TriggerInteraction = value;
        }
        public bool Cast()
        {
            var origin = _Root.TransformPoint(_LocalOrigin);

            bool hit = PhysicsCaster.Cast(origin, _Direction, _Config, out _Hit);
            if (!hit) _Hit = default;

            return hit;
        }

        public void SetOrigin(Vector3 worldPosition) => _LocalOrigin = _Root.InverseTransformPoint(worldPosition);

        public void SetDirection(Vector3 direction) => _Direction = direction.normalized;

        public bool HasHit => _Hit.collider != null;
        public float Distance => _Hit.distance;
        public Vector3 Normal => _Hit.normal;
        public Vector3 Position => _Hit.point;
        public Collider Collider => _Hit.collider;
        public Transform Transform => _Hit.transform;
    }
}

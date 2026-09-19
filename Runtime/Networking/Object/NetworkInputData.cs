using Dave6.LootShooter.Character;
using Unity.Netcode;
using UnityEngine;

namespace Dave6.LootShooter.Networking.Object
{
    public struct NetworkInputData : INetworkSerializable
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

        public NetworkInputData(PlayerInputData input)
        {
            Move = input.Move;
            Look = input.Look;
            Jump = input.Jump;
            Sprint = input.Sprint;
            Crouch = input.Crouch;

            Interact = input.Interact;

            Fire = input.Fire;
            Aim = input.Aim;
            Reload = input.Reload;

            CameraForward = input.CameraForward;
            CameraRight = input.CameraRight;
        }
        public PlayerInputData ToInputData()
        {
            return new PlayerInputData
            {
                Move = Move,
                Look = Look,
                Jump = Jump,
                Sprint = Sprint,
                Crouch = Crouch,

                Interact = Interact,

                Fire = Fire,
                Aim = Aim,
                Reload = Reload,

                CameraForward = CameraForward,
                CameraRight = CameraRight
            };
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref Move);
            serializer.SerializeValue(ref Look);
            serializer.SerializeValue(ref Jump);
            serializer.SerializeValue(ref Sprint);
            serializer.SerializeValue(ref Crouch);

            serializer.SerializeValue(ref Interact);
            
            serializer.SerializeValue(ref Fire);
            serializer.SerializeValue(ref Aim);
            serializer.SerializeValue(ref Reload);

            serializer.SerializeValue(ref CameraForward);
            serializer.SerializeValue(ref CameraRight);
        }
    }
}
namespace Dave6.LootShooter.Character.Network
{
    public sealed class NetworkAgent
    {
        ICharacterNetwork _Network;

        public void Initialize(ICharacterNetwork network)
        {
            _Network = network;
        }

        public void RequestFire()
        {
            _Network?.RequestFire();
        }
    }
}
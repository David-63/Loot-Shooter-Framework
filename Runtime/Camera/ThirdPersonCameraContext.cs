namespace Dave6.LootShooter.Camera
{
    public sealed class ThirdPersonCameraContext
    {
        public float InputYaw;
        public float InputPitch;

        public float AimYaw;
        public float AimPitch;

        public float FinalYaw;
        public float FinalPitch;

        public ThirdPersonPreset TargetPreset = new();
    }
}

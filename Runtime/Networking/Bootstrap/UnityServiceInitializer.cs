using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;

namespace Dave6.LootShooter.Networking.Bootstrap
{
    public sealed class UnityServiceInitializer
    {
        public async Task InitializeAsync()
        {
            if (UnityServices.State != ServicesInitializationState.Initialized)
            {
                await UnityServices.InitializeAsync();
            }

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
        }
    }
}
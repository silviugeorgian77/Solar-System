using System.Threading.Tasks;
#if UNITY_EDITOR
using ParrelSync;
#endif
using Unity.Services.Authentication;
using Unity.Services.Core;

public static class UnityServicesUtils
{
    public static async Task InitializeAsync()
    {
        try
        {
            var options = new InitializationOptions();

#if UNITY_EDITOR
            // Remove this if you don't have ParrelSync installed. 
            // It's used to differentiate the clients,
            // otherwise lobby will count them as the same
            options.SetProfile(
                ClonesManager.IsClone() ?
                ClonesManager.GetArgument() : "Primary"
            );
#endif
            await UnityServices.InitializeAsync(options);

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
        }
        catch
        {
            
        }
    }
}


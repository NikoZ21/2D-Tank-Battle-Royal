using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public static class AuthenticationWrapper
{
    public static AuthState AuthState { get; private set; } = AuthState.NotAuthenticated;

    public static async Task<AuthState> DoAuth(int maxTries = 5)
    {
        if (AuthState == AuthState.Authenticated) return AuthState;

        if (AuthState == AuthState.Authenticating)
        {
            await Authenticating();
            return AuthState;
        }

        await SignInAnonymouslyAsync(maxTries);

        return AuthState;

    }

    public static async Task SignInAnonymouslyAsync(int maxTries)
    {
        AuthState = AuthState.Authenticating;

        int tries = 0;

        while (AuthState == AuthState.Authenticating && tries < maxTries)
        {
            try
            {
                var authentincationService = AuthenticationService.Instance;

                await authentincationService.SignInAnonymouslyAsync();

                if (authentincationService.IsSignedIn && authentincationService.IsAuthorized)
                {
                    AuthState = AuthState.Authenticated;
                    break;
                }

            }
            catch (AuthenticationException ex)
            {
                Debug.LogError(ex.Message);
                AuthState = AuthState.Error;
            }
            catch (RequestFailedException ex)
            {
                Debug.LogError(ex.Message);
                AuthState = AuthState.Error;
            }

            tries++;

            await Task.Delay(1000);
        }

        if (AuthState != AuthState.Authenticated)
        {
            Debug.LogWarning("Player had problem authenticating...");
            AuthState = AuthState.TimeOut;
        }
    }

    private static async Task<AuthState> Authenticating()
    {
        while (AuthState == AuthState.Authenticating || AuthState == AuthState.NotAuthenticated)
        {
            await Task.Delay(200);
        }
        return AuthState;
    }
}

public enum AuthState
{
    NotAuthenticated,
    Authenticating,
    Authenticated,
    Error,
    TimeOut
}


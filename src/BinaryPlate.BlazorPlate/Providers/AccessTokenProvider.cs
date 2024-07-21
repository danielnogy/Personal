﻿namespace BinaryPlate.BlazorPlate.Providers;

public class AccessTokenProvider(ILocalStorageService localStorage)
{
    #region Public Methods

    public async Task<string> TryGetAccessToken()
    {
        return await localStorage.GetItemAsync<string>("AccessToken");
    }

    #endregion Public Methods
}
﻿namespace BinaryPlate.BlazorPlate.Features.AppSettings.Queries.GetSettings.GetTokenSettings;

public class GetTokenSettingsForEditResponse
{
    #region Public Properties

    public string Id { get; set; }
    public int AccessTokenUoT { get; set; }
    public double? AccessTokenTimeSpan { get; set; }
    public int RefreshTokenUoT { get; set; }
    public double? RefreshTokenTimeSpan { get; set; }
    public string ConcurrencyStamp { get; set; }

    #endregion Public Properties
}
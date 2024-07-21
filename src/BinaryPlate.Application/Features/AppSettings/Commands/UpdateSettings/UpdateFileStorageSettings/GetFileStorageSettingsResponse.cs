﻿namespace BinaryPlate.Application.Features.AppSettings.Commands.UpdateSettings.UpdateFileStorageSettings;

public class GetFileStorageSettingsResponse
{
    #region Public Properties

    public Guid Id { get; set; }
    public string SuccessMessage { get; set; }
    public string ConcurrencyStamp { get; set; }

    #endregion Public Properties
}
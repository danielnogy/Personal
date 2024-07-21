namespace BinaryPlate.Infrastructure.Services.Storage;

public class StorageProvider(IStorageFactory storageFactory, IAppSettingsService appSettingsService) : IStorageProvider
{
    #region Public Methods

    public async Task<IFileStorageService> InvokeStorageService()
    {
        var storageTypeResponse = await appSettingsService.GetFileStorageSettings();
        var storageType = storageTypeResponse.Payload.StorageType;
        return storageFactory.CreateInstance((StorageTypes)storageType);
    }

    #endregion Public Methods
}
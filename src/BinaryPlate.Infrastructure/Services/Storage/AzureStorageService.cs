using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;

namespace BinaryPlate.Infrastructure.Services.Storage;

public class AzureStorageService(IConfiguration configuration, IUtcDateTimeProvider utcDateTimeProvider, ITenantResolver tenantResolver) : IFileStorageService
{
    #region Private Fields

    private readonly string _connectionString = configuration.GetConnectionString("AzureStorageConnection");

    #endregion Private Fields

    #region Public Methods

    public async Task<FileMetaData> UploadFile(IFormFile formFile, string relativePath, bool fileRenameAllowed = false, string baseUri = null, CancellationToken cancellationToken = default)
    {
        //var credentials = new StorageCredentials("ssmmaterials", "nGKSVnlCYrS16DAr4CEXNWO3Nv+l4gWDC8a/jxzyLHu+bQPZHyLdwFPdZqmBqCYejEzSfo11J1HB+ASt4+ocgA==");
        //var account = new CloudStorageAccount(credentials, true);
        //var client = account.CreateCloudBlobClient();
        //var properties = await client.GetServicePropertiesAsync();
        //properties.DefaultServiceVersion = "2024-08-04";
        //await client.SetServicePropertiesAsync(properties);
        // Check if file is null or empty.
        if (formFile is { Length: > 0 })
            try
            {
                // Adjust the relative path of the directory with tenant awareness.
                relativePath = ConstructRelativePath(relativePath, tenantResolver);

                // Create a blob container based on the modified relative path.
                // Replace '/' with '-' and convert to lowercase to form a valid container name.
                var containerClient = await CreateBlobContainer(relativePath.Replace('/', '-').ToLower(), cancellationToken);

                // Generate a unique file name using a Unix Time.
                var fileName = $"{utcDateTimeProvider.GetUnixTimeMilliseconds()}";

                // Rename file if allowed.
                if (fileRenameAllowed)
                    fileName = $"{formFile.FileName.ToUrlFriendlyString()}-{fileName}";

                // Append file extension to the generated file name.
                fileName = $"{fileName}{Path.GetExtension(formFile.FileName)}";

                // Get a reference to the blob client with the specified file name.
                var blobClient = containerClient.GetBlobClient(fileName);

                // Upload the file to the blob storage.
                await blobClient.UploadAsync(formFile.OpenReadStream(), cancellationToken: cancellationToken);

                // Return the meta data of the uploaded file.
                return new FileMetaData
                {
                    FileUri = blobClient.Uri.ToString(),
                    FileName = fileName
                };
            }
            catch
            {
                // Throw an exception if the file upload fails.
                throw new Exception(Resource.File_has_not_been_uploaded);
            }

        // If the file has no content, return an empty FileMetaData object.
        return await Task.FromResult(new FileMetaData());
    }

    public async Task<List<FileMetaData>> UploadMultipleFiles(IList<IFormFile> formFiles,
                                                              string relativePath,
                                                              bool fileRenameAllowed = false,
                                                              int defaultFileIndex = 0,
                                                              string baseUrl = null,
                                                              CancellationToken cancellationToken = default)
    {
        var filePaths = new List<FileMetaData>();

        // If there are no form files to upload, return an empty list.
        if (formFiles == null || formFiles.Count == 0)
            return new List<FileMetaData>();

        // Upload each form file to the specified blob storage container.
        foreach (var formFile in formFiles.Select((value, index) => new { Index = index, Value = value }))
            if (formFile.Value.Length > 0)
                try
                {
                    // Adjust the relative path of the directory with tenant awareness.
                    relativePath = ConstructRelativePath(relativePath, tenantResolver);

                    // Create a blob container based on the modified relative path.
                    // Replace '/' with '-' and convert to lowercase to form a valid container name.
                    var containerClient = await CreateBlobContainer(relativePath.Replace('/', '-').ToLower(), cancellationToken);

                    // Generate a unique file name using a Unix Time.
                    var fileName = $"{utcDateTimeProvider.GetUnixTimeMilliseconds()}";

                    // Rename file if allowed.
                    if (fileRenameAllowed)
                        fileName = $"{formFile.Value.FileName.ToUrlFriendlyString()}-{fileName}";

                    // Append file extension to the generated file name.
                    fileName = $"{fileName}{Path.GetExtension(formFile.Value.FileName)}";

                    // Get a reference to the blob client with the specified file name.
                    var blobClient = containerClient.GetBlobClient(fileName);

                    // Upload the file to the blob storage.
                    await blobClient.UploadAsync(formFile.Value.OpenReadStream(), cancellationToken: cancellationToken);

                    // Add metadata for the uploaded file to the list of file paths.
                    filePaths.Add(new FileMetaData
                    {
                        FileName = fileName,
                        FileUri = blobClient.Uri.ToString(),
                        IsDefault = defaultFileIndex == formFile.Index
                    });
                }
                catch (Exception e)
                {
                    throw new Exception(e.ToString());
                }
            else
                // Throw an exception if the form file is empty.
                throw new Exception(Resource.File_is_empty);

        return filePaths;
    }

    public async Task<FileMetaData> EditFile(IFormFile formFile, string relativePath, string oldFileUri, string rootFolderName = default, CancellationToken cancellationToken = default)
    {
        // If no new file was provided, return the URI of the old file through FileMetaData object.
        if (formFile == null)
            return new FileMetaData
            {
                FileUri = oldFileUri,
            };

        // If an old file URI was provided, delete the old file.
        if (!string.IsNullOrEmpty(oldFileUri))
            await DeleteFileIfExists(oldFileUri, rootFolderName);

        // Upload the new file.
        return await UploadFile(formFile, relativePath, cancellationToken: cancellationToken);
    }

    public async Task DeleteFileIfExists(string fileUri, string containerName = default)
    {
        // Check if the file URI is not empty or null.
        if (!string.IsNullOrEmpty(fileUri))
        {
            // Create a BlobContainerClient with the connection string and the container name.
            var container = new BlobContainerClient(_connectionString, containerName);

            // Check if the container exists.
            if (await container.ExistsAsync())
            {
                // Set the access policy of the container to PublicAccessType.BlobContainer.
                await container.SetAccessPolicyAsync(PublicAccessType.BlobContainer);

                // Split the file URI into an array and get the last element (which should be the
                // file name).
                var fileUriArray = fileUri.Split('/').ToArray();
                var fileName = fileUriArray.LastOrDefault();

                // Delete the blob (file) if it exists, including any snapshots.
                await container.DeleteBlobIfExistsAsync(fileName, DeleteSnapshotsOption.IncludeSnapshots);
            }
        }
    }

    public FileStatus GetFileState(IFormFile formFile, string oldUrl)
    {
        // If the IFormFile object is not null or has a length greater than 0, the file is modified.
        if (formFile is not null or { Length: > 0 })
            return FileStatus.Modified;

        // If the old URL is not null or whitespace, the file is unchanged. Otherwise, it's deleted.
        return !string.IsNullOrWhiteSpace(oldUrl) ? FileStatus.Unchanged : FileStatus.Deleted;
    }

    #endregion Public Methods

    #region Private Methods

    private static async Task DeleteBlobIfExistsAsync(BlobContainerClient container, string prefix, int level)
    {
        // Use GetBlobsByHierarchyAsync method to get a page of blobs with the given prefix, using
        // '/' as the delimiter.
        await foreach (var page in container.GetBlobsByHierarchyAsync(prefix: prefix, delimiter: "/").AsPages())
        {
            // Iterate through the blobs in the page and delete them one by one using the
            // DeleteBlobIfExistsAsync method of the container.
            foreach (var blob in page.Values.Where(item => item.IsBlob).Select(item => item.Blob))
                await container.DeleteBlobIfExistsAsync(blob.Name);

            // Get the prefixes in the page and recursively call the DeleteBlobIfExistsAsync method
            // on them to delete all blobs under that prefix.
            var prefixes = page.Values.Where(item => item.IsPrefix).Select(item => item.Prefix);
            foreach (var p in prefixes)
                await DeleteBlobIfExistsAsync(container, p, level + 1);
        }
    }

    /// <summary>
    /// Constructs the relative of the directory with tenant awareness.
    /// </summary>
    /// <param name="relativePath">The relative of the directory.</param>
    /// <param name="tenantResolver">The tenant resolver to determine the tenant-specific path.</param>
    /// <returns>The tenant-aware relative of the directory.</returns>
    private static string ConstructRelativePath(string relativePath, ITenantResolver tenantResolver)
    {
        // Adjust the relative of the directory based on the tenant mode and whether it is the host.
        relativePath = tenantResolver.TenantMode switch
        {
            TenantMode.SingleTenant => relativePath,
            TenantMode.MultiTenant when tenantResolver.IsHostRequest => $"host/{relativePath}",
            TenantMode.MultiTenant when !tenantResolver.IsHostRequest =>
                $"tenants/{tenantResolver.GetTenantName()}/{relativePath}",
            _ => relativePath
        };

        // Return the tenant-aware relative of the directory.
        return relativePath;
    }

    /// <summary>
    /// Creates a new BlobContainerClient instance and ensures the container exists with public blob access.
    /// </summary>
    /// <param name="containerName">The name of the blob container.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BlobContainerClient instance.</returns>
    private async Task<BlobContainerClient> CreateBlobContainer(string containerName, CancellationToken cancellationToken)
    {
        // Create a new BlobContainerClient instance with the specified connection string and directory path.
        var containerClient = new BlobContainerClient(_connectionString, containerName);

        // Create the container if it doesn't exist.
        await containerClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

        // Set the access policy to allow public access to blobs within the container.
        await containerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer, cancellationToken: cancellationToken);

        // Return the BlobContainerClient instance.
        return containerClient;
    }

    #endregion Private Methods
}
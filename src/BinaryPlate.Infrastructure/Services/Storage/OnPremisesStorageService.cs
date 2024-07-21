namespace BinaryPlate.Infrastructure.Services.Storage;

public class OnPremisesStorageService(IWebHostEnvironment env, IUtcDateTimeProvider utcDateTimeProvider, ITenantResolver tenantResolver, IHttpContextAccessor httpContextAccessor) : IFileStorageService
{
    #region Private Fields

    private string _hostUri;
    private string _fileUri;

    #endregion Private Fields

    #region Public Methods

    public async Task<FileMetaData> UploadFile(IFormFile formFile, string relativePath, bool fileRenameAllowed = false, string baseUri = null, CancellationToken cancellationToken = default)
    {
        // Check if the file has content.
        if (formFile is { Length: > 0 })
            try
            {
                // Adjust the relative path of the directory with tenant awareness.
                relativePath = ConstructRelativePath(relativePath, tenantResolver);

                // Combine the relative path of the directory and the web root path to get the full absolute directory path.
                var absolutePath = Path.Combine(env.WebRootPath, relativePath);

                // Create the directory if it doesn't exist.
                Directory.CreateDirectory(absolutePath);

                // Generate a unique file name using a Unix Time.
                var fileName = $"{utcDateTimeProvider.GetUnixTimeMilliseconds()}";

                // If file renaming is allowed, replace spaces and special characters in the file name with dashes.
                if (fileRenameAllowed)
                    fileName = $"{formFile.FileName.ToUrlFriendlyString()}-{fileName}";

                // Add the file extension to the file name.
                fileName = $"{fileName}{Path.GetExtension(formFile.FileName)}";

                // Combine the directory and file name to get the full physical path of the file.
                var physicalPath = Path.Combine(absolutePath, fileName);

                // Create the file on disk.
                await using var stream = File.Create(physicalPath);

                // Copy the contents of the uploaded file to the newly created file.
                await formFile.CopyToAsync(stream, cancellationToken: cancellationToken);

                // Generate the public URL of the file by combining the protocol, host, directory path, and file name.

                var httpContext = httpContextAccessor.HttpContext;

                if (httpContext is not null)
                {
                    // Get the host from the HTTP context.
                    _hostUri = httpContextAccessor.GetBaseUri();
                    _fileUri = $"{_hostUri}/{relativePath}/{fileName}";
                }
                else
                {
                    // Use the provided base URL as the host.
                    _hostUri = baseUri;
                    _fileUri = $"{_hostUri}/{relativePath}/{fileName}";
                }

                // Return the meta data of the uploaded file.
                return new FileMetaData
                {
                    FileUri = _fileUri,
                    FileName = fileName
                };
            }
            catch (Exception)
            {
                // If an error occurs, throw an exception indicating that the file could not be uploaded.
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
        var filesMetaData = new List<FileMetaData>();

        // If the formFiles list is null or empty, return an empty list.
        if (formFiles == null || formFiles.Count == 0)
            return new List<FileMetaData>();

        // Adjust the relative of the directory with tenant awareness.
        relativePath = ConstructRelativePath(relativePath, tenantResolver);

        // Combine the directory path with the WebRootPath to get the full path of the directory.
        var directory = Path.Combine(env.WebRootPath, relativePath);

        // Create the directory if it does not exist.
        Directory.CreateDirectory(directory);

        // Loop through each form file and upload it to the server.
        foreach (var formFile in formFiles.Select((value, index) => new { Index = index, Value = value }))
            // If the form file has a non-zero length, upload it to the server.
            if (formFile.Value.Length > 0)
                try
                {
                    // Generate a unique file name using a Unix Time.
                    var fileName = $"{utcDateTimeProvider.GetUnixTimeMilliseconds()}";

                    // If fileRenameAllowed is true, append the original file name to the generated
                    // file name (with spaces and special characters replaced by dashes).
                    if (fileRenameAllowed)
                        fileName = $"{formFile.Value.FileName.ToUrlFriendlyString()}-{fileName}";

                    // Append the file extension to the file name.
                    fileName = $"{fileName}{Path.GetExtension(formFile.Value.FileName)}";

                    // Combine the directory and file name to get the full physical path of the file.
                    var physicalPath = Path.Combine(directory, fileName);

                    // Create the file on the server and copy the contents of the form file to it.
                    await using (var stream = File.Create(physicalPath))
                    {
                        await formFile.Value.CopyToAsync(stream, cancellationToken: cancellationToken);
                    }

                    // Generate the public URL of the file by combining the protocol, host, directory path, and file name.
                    var httpContext = httpContextAccessor.HttpContext;

                    if (httpContext is not null)
                    {
                        // Get the host from the HTTP context.
                        _hostUri = httpContextAccessor.GetBaseUri();
                        _fileUri = $"{_hostUri}/{relativePath}/{fileName}";
                    }
                    else
                    {
                        // Use the provided base URL as the host.
                        _hostUri = baseUrl;
                        _fileUri = $"{_hostUri}/{relativePath}/{fileName}";
                    }

                    // Add the file metadata to the list of file paths, marking the default file if
                    // its index matches the defaultFileIndex parameter.
                    filesMetaData.Add(new FileMetaData { FileName = fileName, FileUri = _fileUri, IsDefault = defaultFileIndex == formFile.Index });
                }
                catch
                {
                    // If an error occurs during the upload process, throw an exception with a
                    // localized error message.
                    throw new Exception(Resource.File_has_not_been_uploaded);
                }
            else
                // If the form file has a zero length, throw an exception with a localized error message.
                throw new Exception(Resource.File_is_empty);

        // Return the list of file metadata objects.
        return filesMetaData;
    }

    public async Task<FileMetaData> EditFile(IFormFile formFile,
                                             string relativePath,
                                             string oldFileUri,
                                             string rootFolderName = null,
                                             CancellationToken cancellationToken = default)

    {
        // If the IFormFile is null, return the old file URI.
        if (formFile == null)
            return new FileMetaData
            {
                FileUri = oldFileUri,
            };

        // If the old file URI is not null or empty, delete the file.
        if (!string.IsNullOrEmpty(oldFileUri))
            await DeleteFileIfExists(oldFileUri);

        // Return the result of uploading the new file to the specified directory.
        return await UploadFile(formFile, relativePath, cancellationToken: cancellationToken);
    }

    public Task DeleteFileIfExists(string fileUri, string rootFolderName = default)
    {
        // If the file URI is not null or empty,
        if (!string.IsNullOrEmpty(fileUri))
        {
            // create a new Uri object from the file URI.
            var uri = new Uri(fileUri);

            // Get the local path of the file by combining the WebRootPath and the file URI's
            // relative (trimmed of the leading "/").
            var localPath = Path.Combine(env.WebRootPath, uri.AbsolutePath.TrimStart('/'));

            // If the file exists, delete it.
            if (File.Exists(localPath))
                File.Delete(localPath);
        }

        // Return a completed task with the value 0.
        return Task.FromResult(0);
    }

    public Task DeleteDirectory(string directoryPath)
    {
        // Combine the directory path with the WebRootPath to get the full path of the directory.
        var path = Path.Combine(env.WebRootPath, directoryPath);

        // If the directory exists, delete it (including its contents).
        if (Directory.Exists(path))
            Directory.Delete(path, true);

        // Return a completed task.
        return Task.CompletedTask;
    }

    public FileStatus GetFileState(IFormFile formFile, string oldUrl)
    {
        // If the formFile is not null and has a length greater than 0, return FileStatus.Modified.
        if (formFile is not null or { Length: > 0 })
            return FileStatus.Modified;

        // If the oldUrl is not null or whitespace, return FileStatus.Unchanged; otherwise, return FileStatus.Deleted.
        return !string.IsNullOrWhiteSpace(oldUrl) ? FileStatus.Unchanged : FileStatus.Deleted;
    }

    #endregion Public Methods

    #region Private Methods

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

    #endregion Private Methods
}
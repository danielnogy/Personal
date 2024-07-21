namespace BinaryPlate.Application.Common.Enums;

/// <summary>
/// Represents the enumeration for specifying the status of a file.
/// </summary>
public enum FileStatus
{
    /// <summary>
    /// Indicates that the file remains unchanged.
    /// </summary>
    Unchanged = 1,

    /// <summary>
    /// Indicates that the file has been modified.
    /// </summary>
    Modified = 2,

    /// <summary>
    /// Indicates that the file has been deleted.
    /// </summary>
    Deleted = 3
}

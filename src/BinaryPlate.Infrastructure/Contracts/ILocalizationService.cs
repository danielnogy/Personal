namespace BinaryPlate.Infrastructure.Contracts;

/// <summary>
/// This interface represents the contract for fetching the localized text of the application resources.
/// </summary>
public interface ILocalizationService
{
    #region Public Methods

    /// <summary>
    /// Gets the localized string for the specified key.
    /// </summary>
    /// <param name="key">The key for the localized string.</param>
    /// <returns>The localized string.</returns>
    string GetString(string key);

    #endregion Public Methods
}
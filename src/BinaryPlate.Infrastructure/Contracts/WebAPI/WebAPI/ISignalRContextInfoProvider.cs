using Microsoft.AspNetCore.SignalR;

namespace BinaryPlate.Infrastructure.Contracts.WebAPI.WebAPI;

/// <summary>
/// This interface represents the contract for managing SignalR application context.
/// <summary>
/// This interface represents the contract for providing context information related to SignalR connections.
/// </summary>
public interface ISignalRContextInfoProvider
{
    #region Public Methods

    /// <summary>
    /// Gets the host name of the SignalR connection.
    /// </summary>
    /// <param name="hubCallerContext">The context of the SignalR connection.</param>
    /// <returns>The host name of the SignalR connection.</returns>
    string GetHostName(HubCallerContext hubCallerContext);

    /// <summary>
    /// Gets the user identifier of the user associated with the SignalR connection.
    /// </summary>
    /// <param name="hubCallerContext">The context of the SignalR connection.</param>
    /// <returns>The user identifier of the user associated with the SignalR connection.</returns>
    string GetUserNameIdentifier(HubCallerContext hubCallerContext);

    /// <summary>
    /// Gets the name of the user associated with the SignalR connection.
    /// </summary>
    /// <param name="hubCallerContext">The context of the SignalR connection.</param>
    /// <returns>The name of the user associated with the SignalR connection.</returns>
    string GetUserName(HubCallerContext hubCallerContext);

    /// <summary>
    /// Gets the ID of the tenant associated with the SignalR connection, if any.
    /// </summary>
    /// <param name="hubCallerContext">The context of the SignalR connection.</param>
    /// <returns>
    /// The ID of the tenant associated with the SignalR connection, or null if there is none.
    /// </returns>
    (Guid? TenantId, string TenantName) GetTenantInfo(HubCallerContext hubCallerContext);

    /// <summary>
    /// Sets the tenant ID for the SignalR connection via a tenant resolver.
    /// </summary>
    /// <param name="context">The context of the SignalR connection.</param>
    /// <returns>The tenant resolver containing the updated tenant ID.</returns>
    ITenantResolver SetTenantIdViaTenantResolver(HubCallerContext context);

    /// <summary>
    /// Retrieves the language of the user associated with the specified <see cref="HubCallerContext"/>.
    /// </summary>
    /// <param name="hubCallerContext">The HubCallerContext associated with the user.</param>
    /// <returns>The language of the user as a string.</returns>
    string GetUserLanguage(HubCallerContext hubCallerContext);

    /// <summary>
    /// Retrieves the text direction of the user's language based on the specified <see cref="HubCallerContext"/>.
    /// </summary>
    /// <param name="hubCallerContext">The HubCallerContext associated with the user.</param>
    /// <returns>The text direction of the user's language.</returns>
    TextDirection GetUserLanguageDirection(HubCallerContext hubCallerContext);

    #endregion Public Methods
}
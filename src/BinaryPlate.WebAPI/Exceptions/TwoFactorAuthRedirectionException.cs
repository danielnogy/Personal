namespace BinaryPlate.WebAPI.Exceptions;

public class TwoFactorAuthRedirectionException : Exception
{
    #region Public Constructors

    public TwoFactorAuthRedirectionException() : base("Access to this resource requires two factor authentication")
    {
    }

    public TwoFactorAuthRedirectionException(string message) : base(message)
    {
    }

    public TwoFactorAuthRedirectionException(string message, Exception innerException) : base(message, innerException)
    {
    }

    #endregion Public Constructors
}
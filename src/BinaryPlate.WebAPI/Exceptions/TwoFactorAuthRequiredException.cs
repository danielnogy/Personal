namespace BinaryPlate.WebAPI.Exceptions;

public class TwoFactorAuthRequiredException : Exception
{
    #region Public Constructors

    public TwoFactorAuthRequiredException() : base("To access this resource you need to enable two factor authentication")

    {
    }

    public TwoFactorAuthRequiredException(string message) : base(message)
    {
    }

    public TwoFactorAuthRequiredException(string message, Exception innerException) : base(message, innerException)
    {
    }

    #endregion Public Constructors
}
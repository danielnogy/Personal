namespace BinaryPlate.Domain.Exceptions;

public class AdAccountInvalidException
    (string adAccount, Exception ex) : Exception($"AD Account \"{adAccount}\" is invalid.", ex)
{
    #region Public Constructors

    #endregion Public Constructors
}
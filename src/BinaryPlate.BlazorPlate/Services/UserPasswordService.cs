namespace BinaryPlate.BlazorPlate.Services;

public class UserPasswordService
{
    #region Public Properties

    public bool PasswordVisibility { get; set; }
    public string PasswordInputIcon { get; set; } = Icons.Material.Filled.VisibilityOff;
    public InputType PasswordInput { get; set; } = InputType.Password;

    #endregion Public Properties

    #region Private Properties

    private string UserPassword { get; set; }

    #endregion Private Properties

    #region Public Methods

    public void SetUserPassword(string userPassword)
    {
        UserPassword = userPassword;
    }

    public string GetUserPassword()
    {
        return UserPassword;
    }

    public bool UserPasswordProvided()
    {
        return UserPassword != null;
    }

    public void TogglePasswordVisibility()
    {
        if (PasswordVisibility)
        {
            PasswordVisibility = false;
            PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
            PasswordInput = InputType.Password;
        }
        else
        {
            PasswordVisibility = true;
            PasswordInputIcon = Icons.Material.Filled.Visibility;
            PasswordInput = InputType.Text;
        }
    }

    #endregion Public Methods
}
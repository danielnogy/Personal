namespace BinaryPlate.BlazorPlate.Shared.Dialogs;

public partial class SaveConfirmationDialog
{
    #region Private Properties

    [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

    #endregion Private Properties

    #region Private Methods

    private void Submit() => MudDialog.Close(DialogResult.Ok(true));

    private void Cancel() => MudDialog.Cancel();

    #endregion Private Methods
}
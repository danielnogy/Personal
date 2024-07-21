namespace BinaryPlate.BlazorPlate.Shared;

public partial class AppOverlay
{
    #region Public Properties

    [Inject] public AppStateManager AppStateManager { get; set; }
    [Parameter] public bool OverlayVisible { get; set; }

    #endregion Public Properties
}
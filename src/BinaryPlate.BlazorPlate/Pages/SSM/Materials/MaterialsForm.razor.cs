using BinaryPlate.BlazorPlate.Consumers.HttpClients;
using BinaryPlate.BlazorPlate.Contracts.Consumers.SSM;
using BinaryPlate.BlazorPlate.Features.SSM.MaterialCategories.Queries.GetMaterialCategoriess;
using BinaryPlate.BlazorPlate.Features.SSM.Materials.Queries.GetMaterialForEdit;
using BinaryPlate.BlazorPlate.Features.SSM.Materials.Queries.GetMaterials;
using BinaryPlate.BlazorPlate.Shared.SSM;
using Mapster;
using Syncfusion.Blazor.Popups;

namespace BinaryPlate.BlazorPlate.Pages.SSM.Materials
{
    public partial class MaterialsForm
    {
        #region Public Properties
        [Inject] private IFileUploadClient FileUploadClient { get; set; }
        [Parameter] public string HeaderText { get; set; }
        [Parameter] public int MaterialId { get; set; } = 0;
        [Parameter] public MaterialItem MaterialModel { get; set; } 
        [Parameter] public EventCallback<MaterialItem> MaterialModelChanged { get; set; } 
        [Parameter] public EventCallback SubmitForm { get; set; }
        [Parameter] public string BreadCrumbText { get; set; } = "Editare/Adaugare material";
        private BpFileUpload BpFileUploadReference { get; set; }
        private VideoViewer VideoViewerReference { get; set; }
        private GetMaterialCategoriesQuery GetMaterialCategories { get; set; } = new();
        private List<MaterialCategoryItem> MaterialCategoryItems { get; set; } = new();
        #endregion Public Properties

        #region Private Properties


        [Inject] private BreadcrumbService BreadcrumbService { get; set; }
        [Inject] private SfDialogService DialogService { get; set; }
        [Inject] private IMaterialsClient MaterialsClient { get; set; }
        [Inject] private IMaterialCategoriesClient MaterialCategoriesClient { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }
        [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
        private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }



        #endregion Private Properties
        protected override async Task OnInitializedAsync()
        {
            BreadcrumbService.SetBreadcrumbItems(new List<MudBlazor.BreadcrumbItem>
        {
            new("Acasa", "/"),
            new("SSM", "#",true),
            new("Materiale", "/ssm/Materials"),
            new(BreadCrumbText, "#", true)
        });
        }
        private async Task MaterialCategoriesLoad()
        {
            GetMaterialCategories.RowsPerPage = -1;
            var responseWrapper = await MaterialCategoriesClient.GetMaterialCategories(GetMaterialCategories);
            if (responseWrapper.IsSuccessStatusCode)
            {
                MaterialCategoryItems = responseWrapper.Payload.MaterialCategories.Items.ToList();
            }
        }
        private async Task OnValidSubmit()
        {
            var dialog = await DialogService.ConfirmAsync(
                MaterialId !=0 ?"Confirmati actualizarile facute?": "Confirmati inregistrearea noua?",
                "Confirmare");

            if (dialog)
            {
                await SubmitForm.InvokeAsync();
            }
        }
        private async void MaterialChanged()
        {
            if(VideoViewerReference == null)
            {
                await Task.Delay(30);
            }
            await VideoViewerReference.ReloadVideo();
        }
        private async Task MaterialSelected(StreamContent streamContent)
        {
            using var fileFormData = new MultipartFormDataContent
                                 {
                                     { streamContent, "File", streamContent.Headers.GetValues("FileName").LastOrDefault() ?? throw new ArgumentNullException(nameof(streamContent)) },
                                     { new StringContent(BpFileUploadReference.GetFileRenameAllowed().ToString()), "FileRenameAllowed" }
                                 };
            try
            {
                var responseWrapper = await FileUploadClient.UploadFile(fileFormData);

                if (responseWrapper.IsSuccessStatusCode)
                {
                    Snackbar.Add(Resource.File_has_been_uploaded_successfully, Severity.Success);
                    //UserForEdit.IsAvatarAdded = true;
                    MaterialModel.Url = responseWrapper.Payload.FileUri;
                    await VideoViewerReference.ReloadVideo();
                }
                else
                {
                    EditContextApiExceptionFallback.PopulateFormErrors(responseWrapper.ApiErrorResponse);
                    SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
                }
            }
            catch (OperationCanceledException)
            {
                Snackbar.Add(Resource.File_upload_was_cancelled, Severity.Error);
            }
        }
        private void MaterialUnSelected(StreamContent streamContent)
        {
            //UserForEdit.IsAvatarAdded = false;
            MaterialModel.Url = null;
        }

    }
}
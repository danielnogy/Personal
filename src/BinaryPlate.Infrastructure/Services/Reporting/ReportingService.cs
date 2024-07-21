namespace BinaryPlate.Infrastructure.Services.Reporting;

public class ReportingService(IUtcDateTimeProvider utcDateTimeProvider,
                              IStorageProvider storageProvider,
                              ITenantResolver tenantResolver,
                              IWebHostEnvironment env) : IReportingService
{
    #region Public Methods

    public async Task<FileMetaData> GenerateApplicantsPdfReport(List<ApplicantItem> applicantItems, TextDirection textDirection, string baseUri = null)
    {
        // Set the license type for QuestPDF (Community license).
        QuestPDF.Settings.License = LicenseType.Community;

        // Register the custom font for use in the PDF document.
        FontManager.RegisterFont(stream: File.OpenRead(path: $@"{env.WebRootPath}\fonts\andlso.ttf"));

        // Create data for the PDF document.
        var applicantDocumentData = new ApplicantDocumentData
        {
            ApplicantItems = applicantItems,
            TextDirection = textDirection,
            CompanyName = tenantResolver.GetTenantName(),
            WebRootPath = env.WebRootPath,
        };

        // Create an instance of the PDF document.
        var document = new ApplicantDocument(applicantDocumentData);

        // Generate the PDF and store it in a memory stream.
        var pdfMemoryStream = new MemoryStream();
        document.GeneratePdf(pdfMemoryStream);

        // Initialize the storage service.
        var storageService = await storageProvider.InvokeStorageService();

        // Generate a unique file name for the PDF.
        var pdfFileName = $"{utcDateTimeProvider.GetUnixTimeMilliseconds()}.pdf";

        // Create a FormFile from the memory stream.
        var formFile = new FormFile(pdfMemoryStream, 0, pdfMemoryStream.Length, "application/pdf", pdfFileName);

        // Upload the PDF file to the storage service.
        var fileMetaData = await storageService.UploadFile(formFile, "reports/PDF", fileRenameAllowed: false, baseUri: baseUri);

        // Return the FileMetaData object representing the uploaded PDF file.
        return fileMetaData;
    }

    #endregion Public Methods
}
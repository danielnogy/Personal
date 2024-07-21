namespace BinaryPlate.Infrastructure.Services.Reporting.Documents.Applicants;

public class ApplicantDocumentData
{
    #region Public Properties

    public List<ApplicantItem> ApplicantItems { get; set; }
    public TextDirection TextDirection { get; set; }
    public string CompanyName { get; set; }
    public string WebRootPath { get; set; }

    #endregion Public Properties
}
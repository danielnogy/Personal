namespace BinaryPlate.Application.Features.POC.Applicants.Queries.ExportApplicants;

public class ExportApplicantsQuery : IRequest<Envelope<ExportApplicantsResponse>>
{
    #region Public Properties

    public string SearchText { get; set; }
    public string SortBy { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class ExportApplicantsHandler(IReportDataProvider reportDataProvider,
                                         IReportingService reportingService,
                                         IHttpContextAccessor httpContextAccessor) : IRequestHandler<ExportApplicantsQuery, Envelope<ExportApplicantsResponse>>
    {
        #region Public Methods

        public async Task<Envelope<ExportApplicantsResponse>> Handle(ExportApplicantsQuery request, CancellationToken cancellationToken)
        {
            // Retrieve applicant items based on the export query.
            var applicantItems = await reportDataProvider.GetApplicants(new GetApplicantsQuery
                                                                        {
                                                                            SearchText = request.SearchText,
                                                                            SortBy = request.SortBy
                                                                        });

            // Get the base URL from the HTTP context accessor.
            var baseUrl = httpContextAccessor.GetBaseUri();

            // Generate a PDF report for the applicant items.
            var fileMetaData = await reportingService.GenerateApplicantsPdfReport(applicantItems, httpContextAccessor.GetUserLanguageDirection(), baseUrl);

            // Create the export response payload.
            var payload = new ExportApplicantsResponse
                          {
                              SuccessMessage = string.Format(Resource.The_report_0_is_being_downloaded, fileMetaData.FileName as string),
                              ContentType = fileMetaData.ContentType as string,
                              FileName = fileMetaData.FileName as string,
                              FileUri = fileMetaData.FileUri as string,
                          };

            // Return the result in an envelope.
            return Envelope<ExportApplicantsResponse>.Result.Ok(payload);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}
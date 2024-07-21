namespace BinaryPlate.Infrastructure.Services.Reporting.Documents.Applicants;

public class ApplicantDocument(ApplicantDocumentData documentData) : IDocument
{
    #region Private Properties

    private List<ApplicantItem> Applicants { get; } = documentData.ApplicantItems;
    private string CompanyName { get; } = documentData.CompanyName;
    private string RootPath { get; } = documentData.WebRootPath;
    private TextDirection TextDirection { get; } = documentData.TextDirection;

    #endregion Private Properties

    #region Public Methods

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        foreach (var item in Applicants)
            container.Page(page =>
            {

                switch (TextDirection)
                {
                    case TextDirection.Rtl:
                        page.ContentFromRightToLeft();
                        break;

                    case TextDirection.Ltr:
                        page.ContentFromLeftToRight();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                page.Margin(20);
                page.Size(PageSizes.A4.Landscape());
                page.DefaultTextStyle(p => p.FontFamily(Fonts.Calibri));
                page.Header().Column(column =>
                                     {
                                         column.Item().PaddingBottom(10).Background(Colors.Indigo.Lighten3).Element(ComposeCompanyInfo);
                                         column.Item().ShowOnce().Element((x) => ComposeHeader(x, item));
                                     });
                page.Content().Element((x) => ComposeContent(x, item));
                page.Footer().AlignCenter().Text(text =>
                {
                    text.CurrentPageNumber();
                    text.Span(" / ");
                    text.TotalPages();
                });
            });
    }

    #endregion Public Methods

    #region Private Methods

    private void ComposeCompanyInfo(IContainer container)
    {
        container.Row(row =>
            {
                row.AutoItem().AlignCenter().Padding(5).Column(column =>
                {
                    column.Item().Text(string.Format(Resource.Tenant_Name, CompanyName)).FontSize(16).Bold();
                });
            });
    }

    private void ComposeHeader(IContainer container, ApplicantItem item)
    {
        container.Row(row =>
        {
            switch (TextDirection)
            {
                case TextDirection.Rtl:

                    row.AutoItem().PaddingLeft(30).AlignLeft().Column(column =>
                    {
                        column.Item().AlignCenter().Width(120).Height(120).Image($"{RootPath}/images/person.jpg");
                        column.Item().AlignCenter().Text($"{item.FullName}").FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);
                    });

                    row.AutoItem().PaddingLeft(30).Column(column =>
                    {
                        column.Item().PaddingBottom(10).Text(text =>
                        {
                            text.Span($"{Resource.SSNShort}: ").SemiBold().FontSize(12);
                            text.Span($"{item.Ssn}").FontSize(11);
                        });
                        column.Item().PaddingBottom(10).Text(text =>
                        {
                            text.Span($"{Resource.Date_of_Birth}: ").SemiBold().FontSize(12);
                            text.Span($"{item.DateOfBirth:MMMM dd, yyyy (dddd)}").FontSize(11);
                        });

                        column.Item().PaddingBottom(10).Text(text =>
                        {
                            text.Span($"{Resource.Created_On}: ").SemiBold().FontSize(12);
                            text.Span($"{item.CreatedOn:MMMM dd, yyyy (dddd)}").FontSize(11);
                        });
                    });
                    break;

                case TextDirection.Ltr:

                    row.AutoItem().PaddingRight(30).AlignLeft().Column(column =>
                    {
                        column.Item().AlignCenter().Width(120).Height(120).Image($"{RootPath}/images/person.jpg");
                        column.Item().AlignCenter().Text($"{item.FullName}").FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);
                    });

                    row.AutoItem().PaddingRight(30).Column(column =>
                    {
                        column.Item().PaddingBottom(10).Text(text =>
                        {
                            text.Span($"{Resource.SSNShort}: ").SemiBold().FontSize(12);
                            text.Span($"{item.Ssn}").FontSize(11);
                        });
                        column.Item().PaddingBottom(10).Text(text =>
                        {
                            text.Span($"{Resource.Date_of_Birth}: ").SemiBold().FontSize(12);
                            text.Span($"{item.DateOfBirth:MMMM dd, yyyy (dddd)}").FontSize(11);
                        });

                        column.Item().PaddingBottom(10).Text(text =>
                        {
                            text.Span($"{Resource.Created_On}: ").SemiBold().FontSize(12);
                            text.Span($"{item.CreatedOn:MMMM dd, yyyy (dddd)}").FontSize(11);
                        });
                    });
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }


            row.AutoItem().Column(column =>
            {
                column.Item().PaddingBottom(10).Text(text =>
                {
                    text.Span($"{Resource.Height}: ").SemiBold().FontSize(12);
                    text.Span($"{item.Height}").FontSize(11);
                });
                column.Item().PaddingBottom(10).Text(text =>
                {
                    text.Span($"{Resource.Weight}: ").SemiBold().FontSize(12);
                    text.Span($"{item.Weight}").FontSize(11);
                });
                column.Item().PaddingBottom(10).Text(text =>
                {
                    text.Span($"{Resource.BMI}: ").SemiBold().FontSize(12);
                    text.Span($"{item.Bmi:F2}").FontSize(11);
                });
            });
        });
    }

    private void ComposeContent(IContainer container, ApplicantItem item)
    {
        container.PaddingVertical(40).Column(column =>
        {
            column.Item().Element(ComposeContentTitle);
            column.Item().Element((x) => ComposeTable(x, item));
        });
    }

    private void ComposeContentTitle(IContainer container)
    {
        container.Background(Colors.DeepPurple.Lighten4).AlignCenter().Padding(5).Column(column =>
        {
            column.Item().Text(Resource.Applicant_References_List).FontSize(14);
        });
    }

    private void ComposeTable(IContainer container, ApplicantItem applicantItem)
    {
        var headerStyle = TextStyle.Default.SemiBold();
        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.RelativeColumn();
                columns.ConstantColumn(180);
                columns.ConstantColumn(100);
                columns.RelativeColumn();
            });

            table.Header(header =>
            {
                header.Cell().Text(Resource.FullName).Style(headerStyle).FontSize(11);
                header.Cell().Text(Resource.Job_Title).Style(headerStyle).FontSize(11);
                header.Cell().Text(Resource.Phone_Number).Style(headerStyle).FontSize(11);
                header.Cell().Text(Resource.Address).Style(headerStyle).FontSize(11);
            });

            foreach (var item in applicantItem.References)
            {
                table.Cell().Element(CellStyle).Text(item.Name).FontSize(10);
                table.Cell().Element(CellStyle).Text($"{item.JobTitle}").FontSize(10);
                table.Cell().Element(CellStyle).Text($"{item.Phone}").FontSize(10);
                table.Cell().Element(CellStyle).Text($"{item.Address}").FontSize(10);
                continue;
                static IContainer CellStyle(IContainer container) => container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
            }
        });
    }

    #endregion Private Methods
}
using BinaryPlate.Domain.Entities.SSM;
using Mapster;

namespace BinaryPlate.Application.Features.SSM.Tests.Queries.GetTestForEdit;

public class GetTestForEditResponse : AuditableDto
{
    #region Public Properties

    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }

    public string ConcurrencyStamp { get; set; }

    #endregion Public Properties

    #region Public Methods

    public static GetTestForEditResponse MapFromEntity(Test test)
    {
        return test.Adapt<GetTestForEditResponse>();
    }

    #endregion Public Methods
}
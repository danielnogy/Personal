using BinaryPlate.Domain.Entities.SSM;

namespace BinaryPlate.Application.Features.SSM.Tests.Commands.UpdateTest.EditModels;

public class TestQuestionItemForEdit
{
    #region Public Properties

    public int Id { get; set; }

    public int TestId { get; set; }
    public int QuestionId { get; set; }
    public DateTime? CreatedOn { get; set; }

    #endregion Public Properties
}
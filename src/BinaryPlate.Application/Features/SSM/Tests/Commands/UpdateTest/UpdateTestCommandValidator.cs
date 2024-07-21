using BinaryPlate.Application.Features.SSM.Tests.Commands.CreateTest.AddModels;
using BinaryPlate.Application.Features.SSM.Tests.Commands.UpdateTest.EditModels;

namespace BinaryPlate.Application.Features.SSM.Tests.Commands.UpdateTest;

public class UpdateTestCommandValidator : AbstractValidator<UpdateTestCommand>
{
    #region Public Constructors

    public UpdateTestCommandValidator()
    {
        

        RuleFor(v => v.Title).Cascade(CascadeMode.Stop)
                                 .NotEmpty()
                                 .WithMessage("Titlul este obligatoriu");

        

        RuleForEach(v => v.ModifiedTestQuestions).SetValidator(new TestQuestionItemForEditValidator());

        RuleForEach(v => v.NewTestQuestions).SetValidator(new TestQuestionItemForAddValidator());
    }

    #endregion Public Constructors

    #region Public Classes

    public class TestQuestionItemForEditValidator : AbstractValidator<TestQuestionItemForEdit>
    {
        #region Public Constructors

        public TestQuestionItemForEditValidator()
        {
            
        }

        #endregion Public Constructors
    }

    public class TestQuestionItemForAddValidator : AbstractValidator<TestQuestionItemForAdd>
    {
        #region Public Constructors

        public TestQuestionItemForAddValidator()
        {
            
        }

        #endregion Public Constructors
    }

    #endregion Public Classes
}
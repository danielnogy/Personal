using BinaryPlate.BlazorPlate.Features.SSM.Questions.Commands.CreateQuestion;

namespace BinaryPlate.BlazorPlate.Features.SSM.Questions.Commands.UpdateQuestion;

public class UpdateQuestionCommandValidator : AbstractValidator<UpdateQuestionCommand>
{
    #region Public Constructors

    public UpdateQuestionCommandValidator()
    {


        RuleFor(v => v.Text).Cascade(CascadeMode.Stop)
                                 .NotEmpty()
                                 .WithMessage("Textul intrebarii este obligatoriu");



        RuleForEach(v => v.ModifiedAnswers).SetValidator(new AnswerItemForEditValidator());

        RuleForEach(v => v.NewAnswers).SetValidator(new AnswerItemForAddValidator());
    }

    #endregion Public Constructors

    #region Public Classes

    public class AnswerItemForEditValidator : AbstractValidator<AnswerItemForEdit>
    {
        #region Public Constructors

        public AnswerItemForEditValidator()
        {
            RuleFor(v => v.Text).Cascade(CascadeMode.Stop)
                                 .NotEmpty()
                                 .WithMessage("Textul raspunsului este obligatoriu");
        }

        #endregion Public Constructors
    }

    public class AnswerItemForAddValidator : AbstractValidator<AnswerItemForAdd>
    {
        #region Public Constructors

        public AnswerItemForAddValidator()
        {
            RuleFor(v => v.Text).Cascade(CascadeMode.Stop)
                                 .NotEmpty()
                                 .WithMessage("Textul raspunsului este obligatoriu");
        }

        #endregion Public Constructors
    }

    #endregion Public Classes
}
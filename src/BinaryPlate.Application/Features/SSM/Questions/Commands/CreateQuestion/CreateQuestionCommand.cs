using BinaryPlate.Domain.Entities.SSM;
using Mapster;

namespace BinaryPlate.Application.Features.SSM.Questions.Commands.CreateQuestion;

public class CreateQuestionCommand : IRequest<Envelope<CreateQuestionResponse>>
{
    #region Public Properties

    public string Text { get; set; }
    public int CategoryId { get; set; }

    public IReadOnlyList<AnswerItemForAdd> AnswerItems { get; set; } = new List<AnswerItemForAdd>();

    #endregion Public Properties

    #region Public Methods

    public Question MapToEntity()
    {
        return new Question();
    }

    #endregion Public Methods

    #region Public Classes

    public class CreateQuestionCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<CreateQuestionCommand, Envelope<CreateQuestionResponse>>
    {
        #region Public Methods

        public async Task<Envelope<CreateQuestionResponse>> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
        {
            // Map the request to an entity.
            var question = request.Adapt<Question>();

            // Add the question to the database context.
            await dbContext.Questions.AddAsync(question, cancellationToken);

            // Save changes to the database.
            await dbContext.SaveChangesAsync(cancellationToken);

            // Create a response with the question ID and a success message.
            var response = new CreateQuestionResponse
            {
                Id = question.Id,
                SuccessMessage = "Intrebare creata cu succes" //Resource.Question_has_been_created_successfully
            };

            // Return a result envelope with the response as the payload.
            return Envelope<CreateQuestionResponse>.Result.Ok(response);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}
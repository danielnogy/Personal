using BinaryPlate.Application.Features.SSM.Questions.Commands.CreateQuestion;
using BinaryPlate.Domain.Entities.SSM;
using Mapster;

namespace BinaryPlate.Application.Features.SSM.Questions.Commands.UpdateQuestion;

public class UpdateQuestionCommand : IRequest<Envelope<string>>
{
    #region Public Properties

    public int Id { get; set; }
    public string Text { get; set; }
    public int CategoryId { get; set; }
    public string ConcurrencyStamp { get; set; }

    public List<AnswerItemForAdd> NewAnswers { get; set; } = new();
    public List<AnswerItemForEdit> ModifiedAnswers { get; set; } = new();
    public List<int> RemovedAnswers { get; set; } = new();

    #endregion Public Properties

    #region Public Methods

    public void MapToEntity(Question question)
    {
        if (question == null)
            throw new ArgumentNullException(nameof(question));

        UpdateAnswers(question);
        
    }

    #endregion Public Methods

    #region Private Methods

    private void UpdateAnswers(Question question)
    {
        AddAnswers(question);

        ModifyAnswers(question);

        RemoveAnswers(question);
    }

    private void RemoveAnswers(Question question)
    {
        if (RemovedAnswers != null)
        {
            foreach (var removedAnswerId in RemovedAnswers)
            {
                var answer = question.Answers.FirstOrDefault(r => r.Id == removedAnswerId);

                if (answer != null)
                    question.Answers.Remove(answer);
            }
        }

    }

    private void ModifyAnswers(Question question)
    {
        if (ModifiedAnswers != null)
        {
            foreach (var modifiedAnswer in ModifiedAnswers)
            {
                var answer = question.Answers.FirstOrDefault(r => r.Id == modifiedAnswer.Id);

                if (answer != null)
                {
                    modifiedAnswer.Adapt(answer);
                }
            }
        }
        
    }

    private void AddAnswers(Question question)
    {
        NewAnswers?.ForEach(answerItemForAdd => question.Answers.Add(answerItemForAdd.Adapt<Answer>()));
    }

    #endregion Private Methods

    #region Public Classes

    public class UpdateQuestionCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<UpdateQuestionCommand, Envelope<string>>
    {
        #region Public Methods

        public async Task<Envelope<string>> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
        {
            
            // Load the question from the database context and include their answers.
            var question = await dbContext.Questions.Include(a => a.Answers)
                                            .Where(a => a.Id == request.Id)
                                            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            // Return an error response if the question is not found.
            if (question == null)
                return Envelope<string>.Result.NotFound("Incarcarea intrebarii a esuat");

            // Map the request to the loaded entity along with the related entities.
            request.Adapt(question);
            request.MapToEntity(question);


            // Update the entity in the database context.
            dbContext.Questions.Update(question);

            // Save changes to the database.
            await dbContext.SaveChangesAsync(cancellationToken);

            // Return a success response with a message.
            return Envelope<string>.Result.Ok("Intrebare actualizata cu succes");
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}
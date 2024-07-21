namespace BinaryPlate.Application.Features.SSM.Questions.Commands.DeleteQuestion;

public class DeleteQuestionCommand : IRequest<Envelope<string>>
{
    #region Public Properties

    public int Id { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class DeleteQuestionCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<DeleteQuestionCommand, Envelope<string>>
    {
        #region Public Methods

        public async Task<Envelope<string>> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
        {
            // Find the question with the given ID.
            var question = await dbContext.Questions.Where(a => a.Id == request.Id).FirstOrDefaultAsync(cancellationToken: cancellationToken);

            // If no question found with the given ID, return 404 NotFound response.
            if (question == null)
                return Envelope<string>.Result.NotFound("Intrebarea nu a fost gasita");

            // Remove the question from the Questions table.
            dbContext.Questions.Remove(question);

            // Save the changes in the database.
            await dbContext.SaveChangesAsync(cancellationToken);

            // Return a success response.
            return Envelope<string>.Result.Ok("Intrebarea a fost stearsa cu succes");
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}
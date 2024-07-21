namespace BinaryPlate.Application.Features.SSM.Tests.Commands.DeleteTest;

public class DeleteTestCommand : IRequest<Envelope<string>>
{
    #region Public Properties

    public int Id { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class DeleteTestCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<DeleteTestCommand, Envelope<string>>
    {
        #region Public Methods

        public async Task<Envelope<string>> Handle(DeleteTestCommand request, CancellationToken cancellationToken)
        {
            // Find the test with the given ID.
            var test = await dbContext.Tests.Where(a => a.Id == request.Id).FirstOrDefaultAsync(cancellationToken: cancellationToken);

            // If no test found with the given ID, return 404 NotFound response.
            if (test == null)
                return Envelope<string>.Result.NotFound("Testul nu a fost gasit");

            // Remove the test from the Tests table.
            dbContext.Tests.Remove(test);

            // Save the changes in the database.
            await dbContext.SaveChangesAsync(cancellationToken);

            // Return a success response.
            return Envelope<string>.Result.Ok("Testul a fost sters cu succes");
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}
namespace BinaryPlate.Application.Features.SSM.Tests.Queries.GetTestForEdit;

public class GetTestForEditQuery : IRequest<Envelope<GetTestForEditResponse>>
{
    #region Public Properties

    public int Id { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class GetTestForEditQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetTestForEditQuery, Envelope<GetTestForEditResponse>>
    {
        #region Public Methods

        public async Task<Envelope<GetTestForEditResponse>> Handle(GetTestForEditQuery request, CancellationToken cancellationToken)
        {
            
            // Retrieve the test from the database using the ID.
            var test = await dbContext.Tests.Where(a => a.Id == request.Id).FirstOrDefaultAsync(cancellationToken: cancellationToken);

            // If the test is not found, return a not found response.
            if (test == null)
                return Envelope<GetTestForEditResponse>.Result.NotFound("Incarcarea testului a esuat");

            // Map the test entity to an test response DTO.
            var testForEditResponse = GetTestForEditResponse.MapFromEntity(test);

            // Return a success response with the test response DTO as the payload.
            return Envelope<GetTestForEditResponse>.Result.Ok(testForEditResponse);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}
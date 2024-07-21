using BinaryPlate.Application.Features.SSM.Tests.Commands.CreateTest.AddModels;
using BinaryPlate.Domain.Entities.SSM;
using Mapster;

namespace BinaryPlate.Application.Features.SSM.Tests.Commands.CreateTest;

public class CreateTestCommand : IRequest<Envelope<CreateTestResponse>>
{
    #region Public Properties

    public string Title { get; set; }
    public string Description { get; set; }

    public IReadOnlyList<TestQuestionItemForAdd> TestQuestionItems { get; set; } = new List<TestQuestionItemForAdd>();
    public IReadOnlyList<TestMaterialItemForAdd> TestMaterialItems { get; set; } = new List<TestMaterialItemForAdd>();
    public IReadOnlyList<TestResultItemForAdd> TestResultItems { get; set; } = new List<TestResultItemForAdd>();


    #endregion Public Properties

    #region Public Methods

    public Test MapToEntity()
    {
        return new Test();
    }

    #endregion Public Methods

    #region Public Classes

    public class CreateTestCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<CreateTestCommand, Envelope<CreateTestResponse>>
    {
        #region Public Methods

        public async Task<Envelope<CreateTestResponse>> Handle(CreateTestCommand request, CancellationToken cancellationToken)
        {
            // Map the request to an entity.
            var test = request.Adapt<Test>();
            test.TestMaterials = request.TestMaterialItems.ToList().Adapt<List<TestMaterial>>();
            test.TestQuestions = request.TestQuestionItems.ToList().Adapt<List<TestQuestion>>();
            test.TestResults = request.TestResultItems.ToList().Adapt<List<TestResult>>();
            // Add the applicant to the database context.
            await dbContext.Tests.AddAsync(test, cancellationToken);

            // Save changes to the database.
            await dbContext.SaveChangesAsync(cancellationToken);

            // Create a response with the applicant ID and a success message.
            var response = new CreateTestResponse
            {
                Id = test.Id,
                SuccessMessage = "Test creat cu succes" //Resource.Test_has_been_created_successfully
            };

            // Return a result envelope with the response as the payload.
            return Envelope<CreateTestResponse>.Result.Ok(response);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}
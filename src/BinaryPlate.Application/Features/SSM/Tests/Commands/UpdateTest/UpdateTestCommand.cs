using BinaryPlate.Application.Features.SSM.Tests.Commands.CreateTest.AddModels;
using BinaryPlate.Application.Features.SSM.Tests.Commands.UpdateTest.EditModels;
using BinaryPlate.Domain.Entities.SSM;
using Mapster;

namespace BinaryPlate.Application.Features.SSM.Tests.Commands.UpdateTest;

public class UpdateTestCommand : IRequest<Envelope<string>>
{
    #region Public Properties

    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }

    public string ConcurrencyStamp { get; set; }
    //TestQuestion
    public List<TestQuestionItemForAdd> NewTestQuestions { get; set; } = new();
    public List<TestQuestionItemForEdit> ModifiedTestQuestions { get; set; } = new();
    public List<int> RemovedTestQuestions { get; set; } = new();

    //TestMaterial
    public List<TestMaterialItemForAdd> NewTestMaterials { get; set; } = new();
    public List<TestMaterialItemForEdit> ModifiedTestMaterials { get; set; } = new();
    public List<int> RemovedTestMaterials { get; set; } = new();

    //TestResult
    public List<TestResultItemForAdd> NewTestResults { get; set; } = new();
    public List<TestResultItemForEdit> ModifiedTestResults { get; set; } = new();
    public List<int> RemovedTestResults { get; set; } = new();

    #endregion Public Properties

    #region Public Methods

    public void MapToEntity(Test test)
    {
        if (test == null)
            throw new ArgumentNullException(nameof(test));

        UpdateTestQuestions(test);
        UpdateTestMaterials(test);
        UpdateTestResults(test);
    }

    #endregion Public Methods

    #region Private Methods
    
    private void UpdateTestQuestions(Test test)
    {
        AddTestQuestions(test);

        ModifyTestQuestions(test);

        RemoveTestQuestions(test);
    }
    private void UpdateTestMaterials(Test test)
    {
        AddTestMaterials(test);

        ModifyTestMaterials(test);

        RemoveTestMaterials(test);
    }
    private void UpdateTestResults(Test test)
    {
        AddTestResults(test);

        ModifyTestResults(test);

        RemoveTestResults(test);
    }
    
    //remove
    private void RemoveTestQuestions(Test test)
    {
        foreach (var removedTestQuestionId in RemovedTestQuestions)
        {
            var testQuestion = test.TestQuestions.FirstOrDefault(r => r.Id == removedTestQuestionId);

            if (testQuestion != null)
                test.TestQuestions.Remove(testQuestion);
        }
    }
    private void RemoveTestMaterials(Test test)
    {
        foreach (var removedTestMaterialId in RemovedTestMaterials)
        {
            var testMaterial = test.TestMaterials.FirstOrDefault(r => r.Id == removedTestMaterialId);

            if (testMaterial != null)
                test.TestMaterials.Remove(testMaterial);
        }
    }
    private void RemoveTestResults(Test test)
    {
        foreach (var removedTestResultId in RemovedTestResults)
        {
            var testResult = test.TestResults.FirstOrDefault(r => r.Id == removedTestResultId);

            if (testResult != null)
                test.TestResults.Remove(testResult);
        }
    }

    //modify
    private void ModifyTestQuestions(Test test)
    {
        foreach (var modifiedTestQuestion in ModifiedTestQuestions)
        {
            var testQuestion = test.TestQuestions.FirstOrDefault(r => r.Id == modifiedTestQuestion.Id);

            if (testQuestion != null)
            {
                modifiedTestQuestion.Adapt(testQuestion);
            }
        }
    }
    private void ModifyTestMaterials(Test test)
    {
        foreach (var modifiedTestMaterial in ModifiedTestMaterials)
        {
            var testMaterial = test.TestMaterials.FirstOrDefault(r => r.Id == modifiedTestMaterial.Id);

            if (testMaterial != null)
            {
                modifiedTestMaterial.Adapt(testMaterial);
            }
        }
    }
    private void ModifyTestResults(Test test)
    {
        foreach (var modifiedTestResult in ModifiedTestResults)
        {
            var testResult = test.TestResults.FirstOrDefault(r => r.Id == modifiedTestResult.Id);

            if (testResult != null)
            {
                modifiedTestResult.Adapt(testResult);
            }
        }
    }

    //add
    private void AddTestQuestions(Test test)
    {
        NewTestQuestions?.ForEach(testQuestionItemForAdd => test.TestQuestions.Add(testQuestionItemForAdd.Adapt<TestQuestion>()));
    }
    private void AddTestMaterials(Test test)
    {
        NewTestMaterials?.ForEach(testMaterialItemForAdd => test.TestMaterials.Add(testMaterialItemForAdd.Adapt<TestMaterial>()));
    }
    private void AddTestResults(Test test)
    {
        NewTestResults?.ForEach(testResultItemForAdd => test.TestResults.Add(testResultItemForAdd.Adapt<TestResult>()));
    }

    #endregion Private Methods

    #region Public Classes

    public class UpdateTestCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<UpdateTestCommand, Envelope<string>>
    {
        #region Public Methods

        public async Task<Envelope<string>> Handle(UpdateTestCommand request, CancellationToken cancellationToken)
        {
            
            // Load the test from the database context and include their testQuestions.
            var test = await dbContext.Tests.Include(a => a.TestQuestions).Include(m=>m.TestMaterials).Include(x=>x.TestResults)
                                            .Where(a => a.Id == request.Id)
                                            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            // Return an error response if the test is not found.
            if (test == null)
                return Envelope<string>.Result.NotFound("Testul nu a putut fi incarcat");

            // Map the request to the loaded entity along with the related entities.
            request.Adapt(test);
            request.MapToEntity(test);


            // Update the entity in the database context.
            dbContext.Tests.Update(test);

            // Save changes to the database.
            await dbContext.SaveChangesAsync(cancellationToken);

            // Return a success response with a message.
            return Envelope<string>.Result.Ok("Test actualizat cu succes");
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}
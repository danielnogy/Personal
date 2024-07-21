using BinaryPlate.Application.Features.SSM.Questions.Commands.CreateQuestion;
using BinaryPlate.Application.Features.SSM.Tests.Commands.CreateTest;
using BinaryPlate.Application.Features.SSM.Tests.Commands.CreateTest.AddModels;
using BinaryPlate.Domain.Entities.SSM;
using Mapster;

namespace BinaryPlate.Application;

public static class DependencyInjection
{
    #region Public Methods

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Adds MediatR to the dependency injection container and configures it to scan the current
        // assembly for services.
        services.AddMediatR(config =>
                            {
                                config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                                config.NotificationPublisher = new TaskWhenAllPublisher();
                            });

        // Adds a transient service for PerformanceBehaviour to the dependency injection container.
        // This is used as a pipeline behavior for MediatR requests.
        // TODO: Uncomment to if you want to measure the request performance.
        //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));

        // Adds a transient service for LoggingBehaviour to the dependency injection container.
        // This is used as a pipeline behavior for MediatR requests.
        // TODO: Uncomment to if you want to log the request info.
        //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));

        // Adds a transient service for ValidationBehavior to the dependency injection container.
        // This is used as a pipeline behavior for MediatR requests.
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        // Adds a transient service for TransactionBehaviour to the dependency injection container.
        // This is used as a pipeline behavior for MediatR requests.
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehaviour<,>));

        // Returns the service collection.
        return services;
    }
    public static void RegisterMapsterConfiguration(this IServiceCollection services)
    {
        TypeAdapterConfig<CreateTestCommand, Test>
            .NewConfig()
            .Map(dest => dest.TestQuestions, src => src.TestQuestionItems.Adapt<TestQuestion>());
        TypeAdapterConfig<CreateTestCommand, Test>
            .NewConfig()
            .Map(dest => dest.TestMaterials, src => src.TestMaterialItems.Adapt<TestMaterial>());
        TypeAdapterConfig<CreateTestCommand, Test>
            .NewConfig()
            .Map(dest => dest.TestResults, src => src.TestResultItems.Adapt<TestResult>());


        
        
        TypeAdapterConfig<CreateQuestionCommand, Question>
            .NewConfig()
            .Map(dest => dest.Answers, src => src.AnswerItems);
    }
    #endregion Public Methods
}
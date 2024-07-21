using BinaryPlate.Domain.Entities.SSM;
using Mapster;

namespace BinaryPlate.Application.Features.SSM.Departments.Commands.CreateDepartment;

public class CreateDepartmentCommand : IRequest<Envelope<CreateDepartmentResponse>>
{
    #region Public Properties

    public string Name { get; set; }


    #endregion Public Properties

    #region Public Methods

    public Department MapToEntity()
    {
        return new Department();
    }

    #endregion Public Methods

    #region Public Classes

    public class CreateDepartmentCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<CreateDepartmentCommand, Envelope<CreateDepartmentResponse>>
    {
        #region Public Methods

        public async Task<Envelope<CreateDepartmentResponse>> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
        {
            // Map the request to an entity.
            var department = request.Adapt<Department>();

            // Add the department to the database context.
            await dbContext.Departments.AddAsync(department, cancellationToken);

            // Save changes to the database.
            await dbContext.SaveChangesAsync(cancellationToken);

            // Create a response with the department ID and a success message.
            var response = new CreateDepartmentResponse
            {
                Id = department.Id,
                SuccessMessage = "Angajat creat cu succes" //Resource.Department_has_been_created_successfully
            };

            // Return a result envelope with the response as the payload.
            return Envelope<CreateDepartmentResponse>.Result.Ok(response);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}
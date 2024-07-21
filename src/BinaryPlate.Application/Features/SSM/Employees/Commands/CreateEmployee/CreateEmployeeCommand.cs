using BinaryPlate.Domain.Entities.SSM;
using Mapster;

namespace BinaryPlate.Application.Features.SSM.Employees.Commands.CreateEmployee;

public class CreateEmployeeCommand : IRequest<Envelope<CreateEmployeeResponse>>
{
    #region Public Properties

    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Adress { get; set; }
    public int DepartmentId { get; set; }

    #endregion Public Properties

    #region Public Methods

    public Employee MapToEntity()
    {
        return new Employee();
    }

    #endregion Public Methods

    #region Public Classes

    public class CreateEmployeeCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<CreateEmployeeCommand, Envelope<CreateEmployeeResponse>>
    {
        #region Public Methods

        public async Task<Envelope<CreateEmployeeResponse>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            // Map the request to an entity.
            var employee = request.Adapt<Employee>();

            // Add the employee to the database context.
            await dbContext.Employees.AddAsync(employee, cancellationToken);

            // Save changes to the database.
            await dbContext.SaveChangesAsync(cancellationToken);

            // Create a response with the employee ID and a success message.
            var response = new CreateEmployeeResponse
            {
                Id = employee.Id,
                SuccessMessage = "Angajat creat cu succes" //Resource.Employee_has_been_created_successfully
            };

            // Return a result envelope with the response as the payload.
            return Envelope<CreateEmployeeResponse>.Result.Ok(response);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}
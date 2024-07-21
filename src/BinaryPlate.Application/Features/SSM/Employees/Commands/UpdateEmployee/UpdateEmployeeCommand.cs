using BinaryPlate.Application.Features.SSM.Employees.Commands.CreateEmployee;
using BinaryPlate.Domain.Entities.SSM;
using Mapster;

namespace BinaryPlate.Application.Features.SSM.Employees.Commands.UpdateEmployee;

public class UpdateEmployeeCommand : IRequest<Envelope<string>>
{
    #region Public Properties

    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Adress { get; set; }
    public int DepartmentId { get; set; }
    public string ConcurrencyStamp { get; set; }



    #endregion Public Properties

    #region Public Methods

    public void MapToEntity(Employee employee)
    {
        if (employee == null)
            throw new ArgumentNullException(nameof(employee));
        
    }

    #endregion Public Methods

    #region Private Methods

    #endregion Private Methods

    #region Public Classes

    public class UpdateEmployeeCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<UpdateEmployeeCommand, Envelope<string>>
    {
        #region Public Methods

        public async Task<Envelope<string>> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            
            // Load the employee from the database context and include their answers.
            var employee = await dbContext.Employees
                                            .Where(a => a.Id == request.Id)
                                            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            // Return an error response if the employee is not found.
            if (employee == null)
                return Envelope<string>.Result.NotFound("Incarcarea anagajatului a esuat");

            // Map the request to the loaded entity along with the related entities.
            request.Adapt(employee);
            request.MapToEntity(employee);


            // Update the entity in the database context.
            dbContext.Employees.Update(employee);

            // Save changes to the database.
            await dbContext.SaveChangesAsync(cancellationToken);

            // Return a success response with a message.
            return Envelope<string>.Result.Ok("Angajat actualizat cu succes");
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}
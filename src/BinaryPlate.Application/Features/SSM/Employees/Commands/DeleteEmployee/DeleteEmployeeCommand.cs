namespace BinaryPlate.Application.Features.SSM.Employees.Commands.DeleteEmployee;

public class DeleteEmployeeCommand : IRequest<Envelope<string>>
{
    #region Public Properties

    public int Id { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class DeleteEmployeeCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<DeleteEmployeeCommand, Envelope<string>>
    {
        #region Public Methods

        public async Task<Envelope<string>> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            // Find the employee with the given ID.
            var employee = await dbContext.Employees.Where(a => a.Id == request.Id).FirstOrDefaultAsync(cancellationToken: cancellationToken);

            // If no employee found with the given ID, return 404 NotFound response.
            if (employee == null)
                return Envelope<string>.Result.NotFound("Angajatul nu a fost gasit");

            // Remove the employee from the Employees table.
            dbContext.Employees.Remove(employee);

            // Save the changes in the database.
            await dbContext.SaveChangesAsync(cancellationToken);

            // Return a success response.
            return Envelope<string>.Result.Ok("Angajatul a fost sters cu succes");
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}
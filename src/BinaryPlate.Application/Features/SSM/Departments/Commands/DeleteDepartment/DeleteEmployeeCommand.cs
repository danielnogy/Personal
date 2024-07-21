namespace BinaryPlate.Application.Features.SSM.Departments.Commands.DeleteDepartment;

public class DeleteDepartmentCommand : IRequest<Envelope<string>>
{
    #region Public Properties

    public int Id { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class DeleteDepartmentCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<DeleteDepartmentCommand, Envelope<string>>
    {
        #region Public Methods

        public async Task<Envelope<string>> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
        {
            // Find the department with the given ID.
            var department = await dbContext.Departments.Where(a => a.Id == request.Id).FirstOrDefaultAsync(cancellationToken: cancellationToken);

            // If no department found with the given ID, return 404 NotFound response.
            if (department == null)
                return Envelope<string>.Result.NotFound("Angajatul nu a fost gasit");

            // Remove the department from the Departments table.
            dbContext.Departments.Remove(department);

            // Save the changes in the database.
            await dbContext.SaveChangesAsync(cancellationToken);

            // Return a success response.
            return Envelope<string>.Result.Ok("Angajatul a fost sters cu succes");
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}
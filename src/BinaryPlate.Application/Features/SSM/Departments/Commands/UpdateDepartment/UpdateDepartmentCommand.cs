using BinaryPlate.Application.Features.SSM.Departments.Commands.CreateDepartment;
using BinaryPlate.Domain.Entities.SSM;
using Mapster;

namespace BinaryPlate.Application.Features.SSM.Departments.Commands.UpdateDepartment;

public class UpdateDepartmentCommand : IRequest<Envelope<string>>
{
    #region Public Properties

    public int Id { get; set; }
    public string Name { get; set; }

    public string ConcurrencyStamp { get; set; }



    #endregion Public Properties

    #region Public Methods

    public void MapToEntity(Department department)
    {
        if (department == null)
            throw new ArgumentNullException(nameof(department));
        
    }

    #endregion Public Methods

    #region Private Methods

    #endregion Private Methods

    #region Public Classes

    public class UpdateDepartmentCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<UpdateDepartmentCommand, Envelope<string>>
    {
        #region Public Methods

        public async Task<Envelope<string>> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
        {
            
            // Load the department from the database context and include their answers.
            var department = await dbContext.Departments
                                            .Where(a => a.Id == request.Id)
                                            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            // Return an error response if the department is not found.
            if (department == null)
                return Envelope<string>.Result.NotFound("Incarcarea anagajatului a esuat");

            // Map the request to the loaded entity along with the related entities.
            request.Adapt(department);
            request.MapToEntity(department);


            // Update the entity in the database context.
            dbContext.Departments.Update(department);

            // Save changes to the database.
            await dbContext.SaveChangesAsync(cancellationToken);

            // Return a success response with a message.
            return Envelope<string>.Result.Ok("Angajat actualizat cu succes");
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}
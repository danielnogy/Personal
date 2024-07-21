namespace BinaryPlate.Application.Features.SSM.Departments.Queries.GetDepartmentForEdit;

public class GetDepartmentForEditQuery : IRequest<Envelope<GetDepartmentForEditResponse>>
{
    #region Public Properties

    public int Id { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class GetDepartmentForEditQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetDepartmentForEditQuery, Envelope<GetDepartmentForEditResponse>>
    {
        #region Public Methods

        public async Task<Envelope<GetDepartmentForEditResponse>> Handle(GetDepartmentForEditQuery request, CancellationToken cancellationToken)
        {
            
            // Retrieve the department from the database using the ID.
            var department = await dbContext.Departments.Where(a => a.Id == request.Id).FirstOrDefaultAsync(cancellationToken: cancellationToken);

            // If the department is not found, return a not found response.
            if (department == null)
                return Envelope<GetDepartmentForEditResponse>.Result.NotFound("Incarcarea angajatului a esuat");

            // Map the department entity to an department response DTO.
            var departmentForEditResponse = GetDepartmentForEditResponse.MapFromEntity(department);

            // Return a success response with the department response DTO as the payload.
            return Envelope<GetDepartmentForEditResponse>.Result.Ok(departmentForEditResponse);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}
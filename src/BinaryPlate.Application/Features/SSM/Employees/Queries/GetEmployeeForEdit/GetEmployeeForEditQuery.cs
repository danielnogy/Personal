namespace BinaryPlate.Application.Features.SSM.Employees.Queries.GetEmployeeForEdit;

public class GetEmployeeForEditQuery : IRequest<Envelope<GetEmployeeForEditResponse>>
{
    #region Public Properties

    public int Id { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class GetEmployeeForEditQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetEmployeeForEditQuery, Envelope<GetEmployeeForEditResponse>>
    {
        #region Public Methods

        public async Task<Envelope<GetEmployeeForEditResponse>> Handle(GetEmployeeForEditQuery request, CancellationToken cancellationToken)
        {
            
            // Retrieve the employee from the database using the ID.
            var employee = await dbContext.Employees.Where(a => a.Id == request.Id).FirstOrDefaultAsync(cancellationToken: cancellationToken);

            // If the employee is not found, return a not found response.
            if (employee == null)
                return Envelope<GetEmployeeForEditResponse>.Result.NotFound("Incarcarea angajatului a esuat");

            // Map the employee entity to an employee response DTO.
            var employeeForEditResponse = GetEmployeeForEditResponse.MapFromEntity(employee);

            // Return a success response with the employee response DTO as the payload.
            return Envelope<GetEmployeeForEditResponse>.Result.Ok(employeeForEditResponse);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}
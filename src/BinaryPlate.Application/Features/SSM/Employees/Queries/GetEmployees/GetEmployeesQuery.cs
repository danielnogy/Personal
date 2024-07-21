namespace BinaryPlate.Application.Features.SSM.Employees.Queries.GetEmployees;

public class GetEmployeesQuery : FilterableQuery, IRequest<Envelope<GetEmployeesResponse>>
{
    #region Public Classes

    public class GetEmployeesQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetEmployeesQuery, Envelope<GetEmployeesResponse>>
    {
        #region Public Methods

        public async Task<Envelope<GetEmployeesResponse>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
        {
            // Start with a query that retrieves all employees from the database.
            var query = dbContext.Employees.AsQueryable();

            // If a search text is provided, filter the employees by their first name or last name.
            if (!string.IsNullOrWhiteSpace(request.SearchText))
                query = query.Where(a => a.Name.Contains(request.SearchText) || a.Email.Contains(request.SearchText));

            // If a sort by field is provided, sort the query by that field; otherwise, sort by
            // first name and then last name.

            query = !string.IsNullOrWhiteSpace(request.SortBy)
                ? query.SortBy(request.SortBy)
                : query.OrderBy(a => a.Name).ThenBy(a => a.Email);

            // Convert the query to a paged list of employee item DTOs.
            var answers = await query.Select(q => EmployeeItem.MapFromEntity(q))
                                            .AsNoTracking()
                                            .ToPagedListAsync(request.PageNumber, request.RowsPerPage);

            // Create an employees response DTO with the list of employee item DTOs.
            var response = new GetEmployeesResponse
            {
                Employees = answers
            };

            // Return a success response with the employees response DTO as the payload.
            return Envelope<GetEmployeesResponse>.Result.Ok(response);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}
namespace BinaryPlate.Application.Features.SSM.Departments.Queries.GetDepartments;

public class GetDepartmentsQuery : FilterableQuery, IRequest<Envelope<GetDepartmentsResponse>>
{
    #region Public Classes

    public class GetDepartmentsQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetDepartmentsQuery, Envelope<GetDepartmentsResponse>>
    {
        #region Public Methods

        public async Task<Envelope<GetDepartmentsResponse>> Handle(GetDepartmentsQuery request, CancellationToken cancellationToken)
        {
            // Start with a query that retrieves all departments from the database.
            var query = dbContext.Departments.AsQueryable();

            // If a search text is provided, filter the departments by their first name or last name.
            if (!string.IsNullOrWhiteSpace(request.SearchText))
                query = query.Where(a => a.Name.Contains(request.SearchText));

            // If a sort by field is provided, sort the query by that field; otherwise, sort by
            // first name and then last name.

            query = !string.IsNullOrWhiteSpace(request.SortBy)
                ? query.SortBy(request.SortBy)
                : query.OrderBy(a => a.Name);

            // Convert the query to a paged list of department item DTOs.
            var answers = await query.Select(q => DepartmentItem.MapFromEntity(q))
                                            .AsNoTracking()
                                            .ToPagedListAsync(request.PageNumber, request.RowsPerPage);

            // Create an departments response DTO with the list of department item DTOs.
            var response = new GetDepartmentsResponse
            {
                Departments = answers
            };

            // Return a success response with the departments response DTO as the payload.
            return Envelope<GetDepartmentsResponse>.Result.Ok(response);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}
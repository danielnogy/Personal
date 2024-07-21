namespace BinaryPlate.Application.Features.SSM.MaterialCategories.Queries.GetMaterialCategories;

public class GetMaterialCategoriesQuery : FilterableQuery, IRequest<Envelope<GetMaterialCategoriesResponse>>
{
    #region Public Classes

    public class GetMaterialCategoriesQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetMaterialCategoriesQuery, Envelope<GetMaterialCategoriesResponse>>
    {
        #region Public Methods

        public async Task<Envelope<GetMaterialCategoriesResponse>> Handle(GetMaterialCategoriesQuery request, CancellationToken cancellationToken)
        {
            // Start with a query that retrieves all materialCategorys from the database.
            var query = dbContext.MaterialCategories.AsQueryable();

            // If a search text is provided, filter the materialCategorys by their first name or last name.
            if (!string.IsNullOrWhiteSpace(request.SearchText))
                query = query.Where(a => a.Name.Contains(request.SearchText));

            // If a sort by field is provided, sort the query by that field; otherwise, sort by
            // first name and then last name.

            query = !string.IsNullOrWhiteSpace(request.SortBy)
                ? query.SortBy(request.SortBy)
                : query.OrderBy(a => a.Name)/*.ThenBy(a => a.LastName)*/;

            // Convert the query to a paged list of materialCategory item DTOs.
            var answers = await query.Select(q => MaterialCategoryItem.MapFromEntity(q))
                                            .AsNoTracking()
                                            .ToPagedListAsync(request.PageNumber, request.RowsPerPage);

            // Create an materialCategorys response DTO with the list of materialCategory item DTOs.
            var response = new GetMaterialCategoriesResponse
            {
                MaterialCategories = answers
            };

            // Return a success response with the materialCategorys response DTO as the payload.
            return Envelope<GetMaterialCategoriesResponse>.Result.Ok(response);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}
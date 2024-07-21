namespace BinaryPlate.Application.Features.SSM.Materials.Queries.GetMaterials;

public class GetMaterialsQuery : FilterableQuery, IRequest<Envelope<GetMaterialsResponse>>
{
    public int MaterialCategoryId { get; set; }
    #region Public Classes

    public class GetMaterialsQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetMaterialsQuery, Envelope<GetMaterialsResponse>>
    {
        #region Public Methods

        public async Task<Envelope<GetMaterialsResponse>> Handle(GetMaterialsQuery request, CancellationToken cancellationToken)
        {
            // Start with a query that retrieves all materials from the database.
            var query = dbContext.Materials.AsQueryable();

            // If a search text is provided, filter the materials by their first name or last name.
            if (!string.IsNullOrWhiteSpace(request.SearchText))
                query = query.Where(a => a.Title.Contains(request.SearchText));

            // If a sort by field is provided, sort the query by that field; otherwise, sort by
            // first name and then last name.
            if (request.MaterialCategoryId != 0)
                query = query.Where(a => a.MaterialCategoryId == request.MaterialCategoryId);

            query = !string.IsNullOrWhiteSpace(request.SortBy)
                ? query.SortBy(request.SortBy)
                : query.OrderBy(a => a.Title);



            // Convert the query to a paged list of material item DTOs.
            var materials = await query.Select(q => MaterialItem.MapFromEntity(q))
                                            .AsNoTracking()
                                            .ToPagedListAsync(request.PageNumber, request.RowsPerPage);

            // Create an materials response DTO with the list of material item DTOs.
            var response = new GetMaterialsResponse
            {
                Materials = materials
            };

            // Return a success response with the materials response DTO as the payload.
            return Envelope<GetMaterialsResponse>.Result.Ok(response);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}
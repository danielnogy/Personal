namespace BinaryPlate.Application.Features.SSM.QuestionCategories.Queries.GetQuestionCategories;

public class GetQuestionCategoriesQuery : FilterableQuery, IRequest<Envelope<GetQuestionCategoriesResponse>>
{
    #region Public Classes

    public class GetQuestionCategoriesQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetQuestionCategoriesQuery, Envelope<GetQuestionCategoriesResponse>>
    {
        #region Public Methods

        public async Task<Envelope<GetQuestionCategoriesResponse>> Handle(GetQuestionCategoriesQuery request, CancellationToken cancellationToken)
        {
            // Start with a query that retrieves all questionCategorys from the database.
            var query = dbContext.QuestionCategories.AsQueryable();

            // If a search text is provided, filter the questionCategorys by their first name or last name.
            if (!string.IsNullOrWhiteSpace(request.SearchText))
                query = query.Where(a => a.Name.Contains(request.SearchText));

            // If a sort by field is provided, sort the query by that field; otherwise, sort by
            // first name and then last name.

            query = !string.IsNullOrWhiteSpace(request.SortBy)
                ? query.SortBy(request.SortBy)
                : query.OrderBy(a => a.Name)/*.ThenBy(a => a.LastName)*/;

            // Convert the query to a paged list of questionCategory item DTOs.
            var answers = await query.Select(q => QuestionCategoryItem.MapFromEntity(q))
                                            .AsNoTracking()
                                            .ToPagedListAsync(request.PageNumber, request.RowsPerPage);

            // Create an questionCategorys response DTO with the list of questionCategory item DTOs.
            var response = new GetQuestionCategoriesResponse
            {
                QuestionCategories = answers
            };

            // Return a success response with the questionCategorys response DTO as the payload.
            return Envelope<GetQuestionCategoriesResponse>.Result.Ok(response);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}
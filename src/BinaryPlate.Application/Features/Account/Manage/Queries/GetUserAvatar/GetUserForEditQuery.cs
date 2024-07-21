namespace BinaryPlate.Application.Features.Account.Manage.Queries.GetUserAvatar;

public class GetUserAvatarForEditQuery : IRequest<Envelope<GetUserAvatarForEditResponse>>
{
    #region Public Classes

    public class GetUserAvatarQueryHandler(ApplicationUserManager userManager,
                                           IHttpContextAccessor httpContextAccessor) : IRequestHandler<GetUserAvatarForEditQuery, Envelope<GetUserAvatarForEditResponse>>
    {
        #region Public Methods

        public async Task<Envelope<GetUserAvatarForEditResponse>> Handle(GetUserAvatarForEditQuery request, CancellationToken cancellationToken)
        {
            // Get the current user ID from the HttpContext.
            var userId = httpContextAccessor.GetUserId();

            // Return BadRequest if user ID is null or empty.
            if (string.IsNullOrEmpty(userId))
                return Envelope<GetUserAvatarForEditResponse>.Result.BadRequest(Resource.Invalid_user_Id);

            // Find the user by their ID.
            var user = await userManager.FindByIdAsync(userId);

            // Return Unauthorized if user is null.
            if (user == null)
                return Envelope<GetUserAvatarForEditResponse>.Result.Unauthorized(Resource.Unable_to_load_user);

            // Create a GetUserAvatarForEditResponse object with the user's avatar URI.
            var payload = new GetUserAvatarForEditResponse
            {
                AvatarUri = user.AvatarUri
            };

            // Return Ok with the GetUserAvatarForEditResponse object.
            return Envelope<GetUserAvatarForEditResponse>.Result.Ok(payload);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}
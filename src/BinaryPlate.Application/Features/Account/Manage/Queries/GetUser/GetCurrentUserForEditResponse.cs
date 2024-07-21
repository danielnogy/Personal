﻿namespace BinaryPlate.Application.Features.Account.Manage.Queries.GetUser;

public class GetCurrentUserForEditResponse
{
    #region Public Properties

    public string Username { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string JobTitle { get; set; }
    public string AvatarUri { get; set; }
    public List<UserAttachmentItem> AssignedAttachments { get; set; } = new();

    #endregion Public Properties
}
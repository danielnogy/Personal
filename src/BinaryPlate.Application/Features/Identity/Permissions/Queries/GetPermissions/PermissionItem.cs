﻿namespace BinaryPlate.Application.Features.Identity.Permissions.Queries.GetPermissions;

public class PermissionItem
{
    #region Public Properties

    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid? ParentId { get; set; }
    public bool IsChecked { get; set; }
    public bool IsCustomPermission { get; set; }
    public bool HasChildren { get; set; }
    public bool IsRoot { get; set; }

    public List<PermissionItem> Permissions { get; set; } = new();

    #endregion Public Properties

    #region Public Methods

    public static PermissionItem MapFromEntity(ApplicationPermission permission)
    {
        var nestedPermissionItems = permission.Permissions.OrderBy(p => p.Name).Select(MapFromEntity).ToList();

        return new PermissionItem
        {
            Id = permission.Id,
            Name = permission.Name,
            ParentId = permission.ParentId,
            IsRoot = permission.ParentId == null,
            HasChildren = nestedPermissionItems.Any(),
            IsCustomPermission = permission.IsCustomPermission,
            Permissions = nestedPermissionItems
        };
    }

    public static PermissionItem MapFromEntityOnDemand(ApplicationPermission permission)
    {
        return new()
        {
            Id = permission.Id,
            Name = permission.Name,
            ParentId = permission.ParentId,
            IsRoot = permission.ParentId == null,
            HasChildren = permission.Permissions.Any(),
            IsCustomPermission = permission.IsCustomPermission,
            Permissions = permission.Permissions.Select(p => new PermissionItem
            {
                Id = p.Id,
                Name = p.Name,
                ParentId = p.ParentId,
                IsRoot = p.ParentId == null,
                HasChildren = p.Permissions.Any(),
                IsCustomPermission = p.IsCustomPermission,
            }).OrderBy(p => p.Name).ToList()
        };
    }

    #endregion Public Methods
}
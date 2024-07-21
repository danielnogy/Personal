﻿namespace BinaryPlate.BlazorPlate.Services;

public class BreadcrumbService
{
    #region Public Events

    public event EventHandler BreadcrumbChanged;

    #endregion Public Events

    #region Public Properties

    public List<BreadcrumbItem> BreadcrumbItems { get; set; } = new();

    #endregion Public Properties

    #region Public Methods

    public void Reset()
    {
        BreadcrumbItems = new List<BreadcrumbItem>();
    }

    public void SetBreadcrumbItems(List<BreadcrumbItem> breadcrumbItems)
    {
        BreadcrumbItems = breadcrumbItems;
        BreadcrumbChanged?.Invoke(this, EventArgs.Empty);
    }

    #endregion Public Methods
}
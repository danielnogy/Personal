﻿namespace BinaryPlate.Domain.Entities.POC;

/// <summary>
/// Represents a reference for an applicant.
/// </summary>
public class Reference : IAuditable
{
    #region Public Properties

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the job title.
    /// </summary>
    public string JobTitle { get; set; }

    /// <summary>
    /// Gets or sets the phone number.
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// Gets or sets the address.
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the applicant associated with this reference.
    /// </summary>
    public Guid ApplicantId { get; set; }

    /// <summary>
    /// Gets or sets the applicant associated with this reference.
    /// </summary>
    public Applicant Applicant { get; set; }

    public string CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public string ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string DeletedBy { get; set; }

    public DateTime? DeletedOn { get; set; }

    #endregion Public Properties
}
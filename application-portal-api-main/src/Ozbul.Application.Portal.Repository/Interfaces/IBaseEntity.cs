namespace Ozbul.Application.Portal.Repository.Interfaces;

public interface IBaseEntity
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>
    /// The identifier.
    /// </value>
    long Id { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is deleted.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is deleted; otherwise, <c>false</c>.
    /// </value>
    bool IsDeleted { get; set; }

    /// <summary>
    /// Gets or sets a value indicating when this instance is created.
    /// </summary>
    /// <value>
    /// </value>
    DateTime CreatedAt { get; set; }
}
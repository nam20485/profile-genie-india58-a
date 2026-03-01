using ProfileGenie.Core.Entities;

namespace ProfileGenie.Core.Interfaces;

/// <summary>
/// Interface for profile-related operations.
/// </summary>
public interface IProfileService
{
    Task<Profile?> GetByIdAsync(Guid id);
    Task<IEnumerable<Profile>> GetAllAsync();
    Task<Profile> CreateAsync(Profile profile);
    Task<Profile> UpdateAsync(Profile profile);
    Task DeleteAsync(Guid id);
}

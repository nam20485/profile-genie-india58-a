using ProfileGenie.Core.Entities;
using ProfileGenie.Core.Interfaces;

namespace ProfileGenie.Core.Services;

/// <summary>
/// Placeholder implementation of profile service.
/// </summary>
public class ProfileService : IProfileService
{
    // TODO: Inject repository and implement actual data access
    
    public Task<Profile?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Profile>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Profile> CreateAsync(Profile profile)
    {
        throw new NotImplementedException();
    }

    public Task<Profile> UpdateAsync(Profile profile)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}

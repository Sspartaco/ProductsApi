using Products.Library.Contracts.DTO;
using Products.Library.Contracts.Helpers;

namespace Products.Library.Contracts.Services;

public interface IGitHubService
{
    Task<OperationResult<GitHubUserDto>> GetUserAsync(string username);
}

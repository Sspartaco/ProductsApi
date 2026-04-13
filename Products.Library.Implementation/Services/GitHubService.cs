using System.Net;
using System.Text.Json;
using Products.Library.Contracts.DTO;
using Products.Library.Contracts.Helpers;
using Products.Library.Contracts.Services;

namespace Products.Library.Implementation.Services;

public class GitHubService(HttpClient httpClient) : IGitHubService
{
    private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public async Task<OperationResult<GitHubUserDto>> GetUserAsync(string username)
    {
        var response = await httpClient.GetAsync($"users/{username}");

        if (response.StatusCode == HttpStatusCode.NotFound)
            return new OperationResult<GitHubUserDto>().AddNotFound($"GitHub user '{username}' was not found.");

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var dto = JsonSerializer.Deserialize<GitHubUserDto>(content, _jsonOptions);

        return new OperationResult<GitHubUserDto>().AddResult(dto);
    }
}

using Products.Library.Contracts.Services;
using Products.Presentation.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Products.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GitHubController(IGitHubService gitHubService) : ControllerBase
{
    private readonly IGitHubService _gitHubService = gitHubService;

    /// <summary>Returns a GitHub user's public profile by username.</summary>
    [HttpGet("{username}")]
    public async Task<IActionResult> GetUser(string username)
    {
        var result = await _gitHubService.GetUserAsync(username);
        return result.ToActionResult(this);
    }
}

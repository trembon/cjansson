using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CJansson.Services;
using CJansson.ViewModels.Projects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CJansson.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly IGitHubService gitHubService;
        private readonly IConfiguration configuration;

        public ProjectsController(IGitHubService gitHubService, IConfiguration configuration)
        {
            this.gitHubService = gitHubService;
            this.configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            var repos = await gitHubService.GetPublicRepos(configuration["GitHub:Username"]);
            var latestUpdated = repos.OrderByDescending(x => x.UpdatedAt).Take(3).ToList();
            var topStarred = repos.Where(x => !latestUpdated.Contains(x)).OrderByDescending(x => x.StargazersCount).Take(3).ToList();

            var model = new ProjectListViewModel
            {
                GitHubUser = configuration["GitHub:Username"],
                ReposFetchedAt = repos.FetchedAt,
                Repos = latestUpdated.Union(topStarred).ToList()
            };
            return View(model);
        }
    }
}
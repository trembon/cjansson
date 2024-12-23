using CJansson.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CJansson.Services
{
    public interface IGitHubService
    {
        Task<GitHubRepositoryList> GetPublicRepos(string username);
    }

    public class GitHubService : IGitHubService
    {
        private const string GITHUB_HTTPCLIENT = "github";
        private const string GITHUB_REPO_URL_FORMAT = "https://api.github.com/users/{0}/repos?per_page=100";

        private const string CACHE_REPO_LIST = "cache_github_repo";

        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMemoryCache memoryCache;

        public GitHubService(IHttpClientFactory httpClientFactory, IMemoryCache memoryCache)
        {
            this.httpClientFactory = httpClientFactory;
            this.memoryCache = memoryCache;
        }

        public async Task<GitHubRepositoryList> GetPublicRepos(string username)
        {
            return await memoryCache.GetOrCreateAsync(CACHE_REPO_LIST, async cacheEntry =>
            {
                cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(30);
                cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(6);
                return await GetPublicReposInternal(username);
            });
        }

        private async Task<GitHubRepositoryList> GetPublicReposInternal(string username)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, string.Format(GITHUB_REPO_URL_FORMAT, username));
            request.Headers.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("cjansson.se", typeof(Program).Assembly.GetName().Version.ToString()));
            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));

            var client = httpClientFactory.CreateClient(GITHUB_HTTPCLIENT);

            using var response = await client.SendAsync(request);
            var list = await response.Content.ReadFromJsonAsync<GitHubRepositoryList>();
            list.FetchedAt = DateTime.UtcNow;

            return list;
        }
    }
}
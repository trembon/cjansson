using System;
using System.Collections.Generic;

namespace CJansson.Models
{
    public class GitHubRepositoryList : List<GitHubRepository>
    {
        public DateTime FetchedAt { get; set; }

        public GitHubRepositoryList()
        {
        }
    }
}

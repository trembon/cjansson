using CJansson.Models;
using System;
using System.Collections.Generic;

namespace CJansson.ViewModels.Projects
{
    public class ProjectListViewModel
    {
        public string GitHubUser { get; set; }

        public DateTime ReposFetchedAt { get; set; }

        public IEnumerable<GitHubRepository> Repos { get; set; }
    }
}

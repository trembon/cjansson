using CJansson.Core.ActionResults;
using CJansson.Services;
using CJansson.ViewModels.Blog;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace CJansson.Controllers
{
    public class BlogController : Controller
    {
        private const int PAGE_SIZE = 10;
        private const int RSS_SIZE = 50;
        private const int TEN_MINUTES_IN_SECONDS = 10 * 60;

        private readonly IBlogService blogService;

        public BlogController(IBlogService blogService)
        {
            this.blogService = blogService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            BlogListViewModel model = new BlogListViewModel
            {
                Posts = blogService.GetPosts().OrderByDescending(p => p.Publish)
            };

            return View(model);
        }

        [HttpGet("blog/category/{category}")]
        public ActionResult Category(string category)
        {
            BlogListViewModel model = new BlogListViewModel();
            model.Posts = blogService.GetPosts(category).OrderByDescending(p => p.Publish);

            return View("Index", model);
        }

        [HttpGet("blog/feed.rss")]
        public ActionResult RSS()
        {
            IEnumerable<BlogListPostViewModel> posts = blogService.GetPosts().OrderByDescending(p => p.Publish);

            RssActionResult rss = new RssActionResult("Blog - cjansson.se", "The programming and me", Url.Action("Index"));
            foreach (BlogListPostViewModel post in posts)
                rss.AddItem(post.Name, Url.Action("Post", new { urlSegment = post.URLSegment }), post.Publish, post.Description);

            return rss;
        }

        [HttpGet("blog/category/{category}/feed.rss")]
        public ActionResult RSSCategory(string category)
        {
            IEnumerable<BlogListPostViewModel> posts = blogService.GetPosts(category).OrderByDescending(p => p.Publish);

            RssActionResult rss = new RssActionResult("Blog - cjansson.se", "The programming and me", Url.Action("Category", new { category }));
            foreach (BlogListPostViewModel post in posts)
                rss.AddItem(post.Name, Url.Action("Post", new { urlSegment = post.URLSegment }), post.Publish, post.Description);

            return rss;
        }

        [HttpGet]
        [Route("blog/post/{urlSegment}")]
        public IActionResult Post(string urlSegment, string secret = null)
        {
            BlogPostViewModel model = blogService.GetPost(urlSegment, secret);
            if (model == null)
                return NotFound();

            return View(model);
        }
    }
}
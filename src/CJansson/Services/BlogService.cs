using CJansson.Models;
using CJansson.ViewModels.Blog;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CJansson.Services
{
    public interface IBlogService
    {
        void Build();

        IEnumerable<BlogListPostViewModel> GetPosts();

        IEnumerable<BlogListPostViewModel> GetPosts(string tag);

        BlogPostViewModel GetPost(string url, string secret);

        byte[] GetImage(string blogUrl, string image);

        Tuple<string, string> GetCodeFile(string blogUrl, string code);
    }

    public class BlogService : IBlogService
    {
        private readonly IWebHostEnvironment env;
        private readonly IContentProcessorService contentProcessorService;

        private List<BlogPost> posts;
        private object postsLock;

        public BlogService(IWebHostEnvironment env, IContentProcessorService contentProcessorService)
        {
            this.env = env;
            this.contentProcessorService = contentProcessorService;

            this.posts = new List<BlogPost>();
            this.postsLock = new object();
        }

        public void Build()
        {
            try
            {
                List<BlogPost> newPosts = new List<BlogPost>();

                foreach(string path in Directory.GetDirectories(Path.Combine(env.ContentRootPath, "posts")))
                {
                    BlogPost post = new BlogPost();
                    post.FullPath = path;

                    string folder = Path.GetFileNameWithoutExtension(path);
                    post.URLSegment = folder.Substring(7);
                    post.Created = DateTime.ParseExact(folder.Substring(0, 6), "yyMMdd", CultureInfo.InvariantCulture);
                    post.Changed = File.GetLastWriteTime(Path.Combine(path, "content.md"));

                    string jsonData = File.ReadAllText(Path.Combine(path, "metadata.json"));
                    JsonBlogPost jsonBlogPost = JsonConvert.DeserializeObject<JsonBlogPost>(jsonData);
                    post.Name = jsonBlogPost.Name;
                    post.Description = jsonBlogPost.Description;
                    post.Publish = jsonBlogPost.Publish;
                    post.Secret = jsonBlogPost.Secret;
                    post.Tags = jsonBlogPost.Tags;


                    string[] imageExtensions = new string[] { ".png", ".jpg", ".jpeg" };
                    foreach (string image in Directory.GetFiles(path))
                    {
                        string extension = Path.GetExtension(image);
                        if (!imageExtensions.Contains(extension))
                            continue;

                        string filename = Path.GetFileName(image);
                        post.Images.Add(filename, File.ReadAllBytes(image));
                    }

                    foreach (string file in Directory.GetFiles(path))
                    {
                        string filename = Path.GetFileName(file);
                        if (filename == "metadata.json" || filename == "content.md")
                            continue;

                        if (post.Images.ContainsKey(filename))
                            continue;

                        post.Files.Add(filename.Replace(".", ""), new Tuple<string, string>(filename, File.ReadAllText(file)));
                    }

                    post.SplashImage = post.Images.Keys.FirstOrDefault(k => k.StartsWith("splash."));

                    string content = File.ReadAllText(Path.Combine(path, "content.md"));
                    post.Content = contentProcessorService.Process(content, post);

                    newPosts.Add(post);
                }

                lock (postsLock)
                    posts = newPosts;
            }
            catch(Exception ex)
            {
                Console.Error.WriteLine($"ERROR: {ex.Message}");
            }
        }

        public Tuple<string, string> GetCodeFile(string blogUrl, string code)
        {
            BlogPost post;
            lock (postsLock)
                post = posts.FirstOrDefault(p => p.URLSegment == blogUrl);

            if (post == null && !post.Files.ContainsKey(code))
                return null;

            return post.Files[code];
        }

        public byte[] GetImage(string blogUrl, string image)
        {
            BlogPost post;
            lock (postsLock)
                post = posts.FirstOrDefault(p => p.URLSegment == blogUrl);

            if (post == null && !post.Images.ContainsKey(image))
                return null;

            return post.Images[image];
        }

        public BlogPostViewModel GetPost(string url, string secret)
        {
            lock (postsLock)
            {
                BlogPost post = posts.FirstOrDefault(p => p.URLSegment == url);
                if (post == null)
                    return null;

                if (!post.IsPublished && (post.Secret == null || post.Secret != secret))
                    return null;

                return new BlogPostViewModel
                {
                    Name = post.Name,
                    Description = post.Description,
                    MainBody = post.Content,
                    Publish = post.Publish,
                    Tags = post.Tags,
                    URLSegment = post.URLSegment,
                    SplashImage = post.SplashImage
                };
            }
        }

        public IEnumerable<BlogListPostViewModel> GetPosts()
        {
            lock (postsLock)
            {
                return posts
                    .Where(p => p.IsPublished)
                    .Select(p => new BlogListPostViewModel
                    {
                        Name = p.Name,
                        Description = p.Description,
                        URLSegment = p.URLSegment,
                        Created = p.Created,
                        Changed = p.Changed,
                        Publish = p.Publish,
                        Tags = p.Tags
                    });
            }
        }

        public IEnumerable<BlogListPostViewModel> GetPosts(string tag)
        {
            lock (postsLock)
            {
                return posts.Where(p => p.IsPublished && p.Tags.Contains(tag, StringComparer.OrdinalIgnoreCase)).Select(p => new BlogListPostViewModel
                {
                    Name = p.Name,
                    Description = p.Description,
                    URLSegment = p.URLSegment,
                    Created = p.Created,
                    Changed = p.Changed,
                    Publish = p.Publish,
                    Tags = p.Tags
                });
            }
        }
    }
}

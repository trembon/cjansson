using CJansson.Core.MarkdownExtensions;
using CJansson.Models;
using Markdig;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CJansson.Services
{
    public interface IContentProcessorService
    {
        string Process(string content, BlogPost blogPost);
    }

    public class ContentProcessorService : IContentProcessorService
    {
        public string Process(string content, BlogPost blogPost)
        {
            MarkdownPipeline pipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .UseAutoIdentifiers()
                .UseAutoLinks()
                .UseSoureCode(blogPost)
                .Build();

            var document = Markdown.Parse(content, pipeline);

            var links = document.Descendants().OfType<LinkInline>();
            foreach (var link in links)
            {
                if (!link.Url.StartsWith("http://") && !link.Url.StartsWith("https://") && !link.Url.StartsWith("/"))
                {
                    if (link.IsImage)
                    {
                        link.Url = $"/files/{blogPost.URLSegment}/image/{link.Url}";
                    }
                }
            }
            
            using (var writer = new StringWriter())
            {
                var renderer = new HtmlRenderer(writer);
                pipeline.Setup(renderer);
                renderer.Render(document);

                return writer.ToString();
            }
        }
    }
}

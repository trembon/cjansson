using CJansson.Models;
using Markdig;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CJansson.Core.MarkdownExtensions
{
    public class SourceCodeExtension : IMarkdownExtension
    {
        private BlogPost blogPost;

        public SourceCodeExtension(BlogPost blogPost)
        {
            this.blogPost = blogPost;
        }

        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            pipeline.BlockParsers.AddIfNotAlready<SourceCodeBlockParser>();
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            var htmlRenderer = renderer as HtmlRenderer;
            if (htmlRenderer != null && !htmlRenderer.ObjectRenderers.Contains<SourceCodeRenderer>())
                htmlRenderer.ObjectRenderers.Insert(0, new SourceCodeRenderer(blogPost));
        }
    }
}

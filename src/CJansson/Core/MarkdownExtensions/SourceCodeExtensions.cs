using CJansson.Models;
using Markdig;
using Markdig.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CJansson.Core.MarkdownExtensions
{
    public static class SourceCodeExtensions
    {
        public static MarkdownPipelineBuilder UseSoureCode(this MarkdownPipelineBuilder pipeline, BlogPost blogPost)
        {
            OrderedList<IMarkdownExtension> extensions = pipeline.Extensions;

            if (!extensions.Contains<SourceCodeExtension>())
                extensions.Add(new SourceCodeExtension(blogPost));

            return pipeline;
        }
    }
}

using CJansson.Models;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CJansson.Core.MarkdownExtensions
{
    public class SourceCodeRenderer : HtmlObjectRenderer<SourceCode>
    {
        private BlogPost blogPost;

        public SourceCodeRenderer(BlogPost blogPost)
        {
            this.blogPost = blogPost;
        }

        protected override void Write(HtmlRenderer renderer, SourceCode obj)
        {
            string file = obj.File.Replace(".", "");
            if (!blogPost.Files.ContainsKey(file))
                return;

            string[] code = blogPost.Files[file].Item2.Split(Environment.NewLine);

            int start = obj.StartLine ?? 1;
            int end = obj.EndLine ?? code.Length;

            // get all the lines that should be showned
            IEnumerable<string> lines = code.Skip(start - 1).Take(end - (start - 1));

            // if no lines was found, dont add any code
            if (lines.Count() == 0)
                return;

            // replaces tabs with 4 spaces
            lines = lines.Select(l => l.Replace("\t", "    "));

            // check if indentation needs to be removed, if its part of a file. only check on none empty rows
            int lowestIndentation = lines.Where(l => !string.IsNullOrWhiteSpace(l)).Min(CalculateStartingSpaceCount);
            if (lowestIndentation > 0)
                lines = lines.Select(l => string.IsNullOrWhiteSpace(l) ? "" : l.Substring(lowestIndentation));
            
            renderer.WriteLine($"<div class=\"sourcecode\">");
            renderer.WriteLine($"<pre class=\"line-numbers\" data-start=\"{start}\" data-filename=\"{obj.File}\" data-url=\"/files/{blogPost.URLSegment}/code/{file}\">");
            renderer.Write($"<code class=\"language-{obj.Language} line-numbers\">");

            for (int i = 0; lines.Count() > i; i++)
            {
                if(i == lines.Count() - 1)
                {
                    renderer.Write(WebUtility.HtmlEncode(lines.ElementAt(i)));
                }
                else
                {
                    renderer.WriteLine(WebUtility.HtmlEncode(lines.ElementAt(i)));
                }
            }

            renderer.WriteLine("</code>");
            renderer.WriteLine("</pre>");
            renderer.WriteLine("</div>");
        }

        private int CalculateStartingSpaceCount(string codeLine)
        {
            for (int i = 0; i < codeLine.Length; i++)
                if (codeLine[i] != ' ')
                    return i;

            return 0;
        }
    }
}

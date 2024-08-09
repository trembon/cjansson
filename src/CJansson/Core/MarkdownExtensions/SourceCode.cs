using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CJansson.Core.MarkdownExtensions
{
    [DebuggerDisplay("$[" + nameof(File) + "](" + nameof(Language) + ")")]
    public class SourceCode : LeafBlock
    {
        public SourceCode(BlockParser parser) : base(parser)
        {
        }

        public string File { get; set; }

        public int? StartLine { get; set; }

        public int? EndLine { get; set; }

        public string Language { get; set; }
    }
}

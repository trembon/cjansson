using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CJansson.Core.MarkdownExtensions
{
    public class SourceCodeBlockParser : BlockParser
    {
        public SourceCodeBlockParser()
        {
            this.OpeningCharacters = new char[] { '$' };
        }

        public override BlockState TryOpen(BlockProcessor processor)
        {
            if (processor.IsCodeIndent)
                return BlockState.None;

            var slice = processor.Line;
            var startPosition = slice.Start;

            var c = slice.NextChar();
            if (c != '[')
                return BlockState.None;
            
            string file;
            if (!LinkHelper.TryParseLabel(ref slice, out file, out SourceSpan fileSpan))
                return BlockState.None;

            c = slice.CurrentChar;
            if (c != '(')
                return BlockState.None;
            
            string settings;
            if (!LinkHelper.TryParseUrl(slice, out settings))
                return BlockState.None;

            string[] splitedSettings = settings.Trim('(', ')').Split(',', StringSplitOptions.RemoveEmptyEntries);
            if (splitedSettings.Length == 0)
                return BlockState.None;

            try
            {
                SourceCode sourceCode = new SourceCode(this)
                {
                    File = file,
                    Language = splitedSettings[0],
                    StartLine = splitedSettings.Length >= 2 ? (int?)int.Parse(splitedSettings[1]) : null,
                    EndLine = splitedSettings.Length >= 3 ? (int?)int.Parse(splitedSettings[2]) : null,
                    Span = new SourceSpan(startPosition, slice.End)
                };

                processor.Document.Add(sourceCode);
            }
            catch
            {
                return BlockState.None;
            }

            return BlockState.BreakDiscard;
        }
    }
}

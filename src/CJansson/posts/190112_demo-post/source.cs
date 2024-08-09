            try
            {
                processor.Document.Add(sourceCode);
            }
            catch
            {
                return BlockState.None;
            }
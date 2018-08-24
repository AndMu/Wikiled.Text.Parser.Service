using Wikiled.Text.Analysis.Structure.Raw;

namespace Wikiled.Text.Parser.Api.Data
{
    public class ParsingResult
    {
        public RawDocument Text { get; set; }

        public string Name { get; set; }
    }
}

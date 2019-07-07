using System.Threading;
using System.Threading.Tasks;
using Wikiled.Text.Parser.Api.Data;

namespace Wikiled.Text.Parser.Api.Service
{
    public interface IDocumentParser
    {
        Task<ParsingResult> Parse(ParsingRequest request, CancellationToken token);
    }
}
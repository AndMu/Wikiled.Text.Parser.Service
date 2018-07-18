using System;
using System.Threading;
using System.Threading.Tasks;
using Wikiled.Text.Parser.Api.Data;

namespace Wikiled.Text.Parser.Api.Service
{
    public interface IDocumentParser : IDisposable
    {
        Task<ParsingResult> Parse(string name, byte[] fileData, CancellationToken token);
    }
}
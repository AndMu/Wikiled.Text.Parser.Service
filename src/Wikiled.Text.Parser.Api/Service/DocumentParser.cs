using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Wikiled.Common.Net.Client;
using Wikiled.Text.Parser.Api.Data;

namespace Wikiled.Text.Parser.Api.Service
{
    public class DocumentParser : IDocumentParser
    {
        private readonly IApiClient client;

        public DocumentParser(IApiClientFactory factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            client = factory.GetClient();
            client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public static IDocumentParser Construct(string server, int port)
        {
            return new DocumentParser(new ApiClientFactory(new HttpClient { Timeout = TimeSpan.FromMinutes(30) },
                                                           new Uri($"http://{server}:{port}")));
        }

        public async Task<ParsingResult> Parse(ParsingRequest request, CancellationToken token)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = await client.PostRequest<ParsingRequest, RawResponse<ParsingResult>>("api/parser/processfile", request, token).ConfigureAwait(false);
            if (!result.IsSuccess)
            {
                throw new ApplicationException("Failed to retrieve:" + result.HttpResponseMessage);
            }

            return result.Result.Value;
        }
    }
}

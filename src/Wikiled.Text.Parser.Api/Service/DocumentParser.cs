using System;
using System.Data;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Wikiled.Common.Net.Client;
using Wikiled.Common.Net.Client.Serialization;
using Wikiled.Text.Parser.Api.Data;

namespace Wikiled.Text.Parser.Api.Service
{
    public class DocumentParser : IDocumentParser
    {
        private readonly ILogger<DocumentParser> logger;

        private readonly HttpClient client;

        private readonly RawResponseDeserializer deserializer = new RawResponseDeserializer();

        public DocumentParser(HttpClient client, Uri serviceUri, ILoggerFactory loggerFactory)
        {
            if (loggerFactory is null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            this.client = client ?? throw new ArgumentNullException(nameof(client));
            client.BaseAddress = serviceUri ?? throw new ArgumentNullException(nameof(serviceUri));
            logger = loggerFactory.CreateLogger<DocumentParser>();
        }

        public async Task<ParsingResult> Parse(string name, byte[] fileData)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (fileData is null)
            {
                throw new ArgumentNullException(nameof(fileData));
            }

            ByteArrayContent bytes = new ByteArrayContent(fileData);
            MultipartFormDataContent multiContent = new MultipartFormDataContent();
            multiContent.Add(bytes, "file", name);
            var result = client.PostAsync("api/parse/processfile", multiContent).Result;
            var data = await deserializer.GetData<RawResponse<ParsingResult>>(result).ConfigureAwait(false);
            if (!data.IsSuccess)
            {
                logger.LogError("Failed to retrieve data {0}", data.HttpResponseMessage.ToString());
                throw new DataException("Failed to retrieve data");
            }

            return data.Result.Value;
        }

        public void Dispose()
        {
            client?.Dispose();
        }
    }
}

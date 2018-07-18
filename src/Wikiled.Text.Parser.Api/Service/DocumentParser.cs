﻿using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Wikiled.Common.Net.Client;
using Wikiled.Text.Parser.Api.Data;

namespace Wikiled.Text.Parser.Api.Service
{
    public class DocumentParser
    {
        private readonly IApiClient client;

        public DocumentParser(IApiClientFactory factory)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            client = factory.GetClient();
            client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<ParsingResult> Parse(string name, byte[] fileData, CancellationToken token)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (fileData is null)
            {
                throw new ArgumentNullException(nameof(fileData));
            }

            ParsingRequest request = new ParsingRequest();
            request.Data = fileData;
            request.Name = name;
            var result = await client.PostRequest<ParsingRequest, RawResponse<ParsingResult>>("api/parser/processfile", request, token).ConfigureAwait(false);
            if (!result.IsSuccess)
            {
                throw new ApplicationException("Failed to retrieve:" + result.HttpResponseMessage);
            }

            return result.Result.Value;
        }
    }
}

using NUnit.Framework;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using Wikiled.Common.Net.Client;
using Wikiled.Text.Analysis.Structure.Raw;
using Wikiled.Text.Parser.Api.Data;
using Wikiled.Text.Parser.Api.Service;

namespace Wikiled.Text.Parser.Service.Tests.Service
{
    [TestFixture]
    public class DocumentParserTests
    {
        private DocumentParser instance;

        private MockHttpMessageHandler mockHttp;

        private HttpClient httpClient;

        private ParsingResult result;

        private IApiClientFactory factory;

        [SetUp]
        public void SetUp()
        {
            result = new ParsingResult();
            result.Name = "Test";
            result.Document = new RawDocument();
            result.Document.Pages = new RawPage[1];
            result.Document.Pages[0] = new RawPage
            {
                Blocks = new[]
                {
                    new TextBlockItem
                    {
                        Text = "Text"
                    }
                }
            };
            mockHttp = new MockHttpMessageHandler();
            httpClient = new HttpClient(mockHttp);
            factory = new ApiClientFactory(httpClient, new Uri("http://localhost"));
            instance = CreateDocumentParser();
        }

        [Test]
        public async Task PostRequest()
        {
            // Setup a respond for the user api (including a wildcard in the URL)
            string output = JsonConvert.SerializeObject(result);
            mockHttp.When("http://localhost/api/parser/processfile")
                    .Respond("application/json", output);
            var request = new ParsingRequest();
            request.Name = "Test";
            request.Data = new byte[] { };
            var actual = await instance.Parse(request, CancellationToken.None).ConfigureAwait(false);
            Assert.AreEqual("Text", actual.Document.Pages[0].Blocks[0].Text);
        }

        [Test]
        public void Construct()
        {
            Assert.Throws<ArgumentNullException>(() => new DocumentParser(null));
        }

        private DocumentParser CreateDocumentParser()
        {
            return new DocumentParser(factory);
        }
    }
}
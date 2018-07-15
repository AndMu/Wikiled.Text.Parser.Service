using NUnit.Framework;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using Wikiled.Text.Parser.Api.Data;
using Wikiled.Text.Parser.Api.Service;

namespace Wikiled.Text.Parser.Service.Tests.Service
{
    [TestFixture]
    public class DocumentParserTests
    {
        private NullLoggerFactory factory;

        private DocumentParser instance;

        private MockHttpMessageHandler mockHttp;

        private HttpClient httpClient;

        private Uri baseUri;

        private ParsingResult result;

        [SetUp]
        public void SetUp()
        {
            factory = new NullLoggerFactory();
            result = new ParsingResult();
            result.Name = "Test";
            result.FileLength = 10;
            result.Text = "Text";
            mockHttp = new MockHttpMessageHandler();
            httpClient = new HttpClient(mockHttp);
            baseUri = new Uri("http://localhost");
            instance = CreateDocumentParser();
        }

        [Test]
        public async Task PostRequest()
        {
            // Setup a respond for the user api (including a wildcard in the URL)
            string output = JsonConvert.SerializeObject(result);
            mockHttp.When("http://localhost/api/parse/processfile")
                    .Respond("application/json", output);
            var actual = await instance.Parse("Test", new byte[] { });
            Assert.AreEqual("Text", actual.Text);
        }

        [Test]
        public void Construct()
        {
            Assert.Throws<ArgumentNullException>(() => new DocumentParser(null, baseUri, factory));
            Assert.Throws<ArgumentNullException>(() => new DocumentParser(httpClient, null, factory));
            Assert.Throws<ArgumentNullException>(() => new DocumentParser(httpClient, baseUri, null));
        }

        private DocumentParser CreateDocumentParser()
        {
            return new DocumentParser(httpClient, baseUri, factory);
        }
    }
}
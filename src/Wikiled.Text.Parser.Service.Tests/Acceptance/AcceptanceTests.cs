using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Wikiled.Common.Net.Client;
using Wikiled.Server.Core.Testing.Server;
using Wikiled.Text.Parser.Api.Data;
using Wikiled.Text.Parser.Api.Service;

namespace Wikiled.Text.Parser.Service.Tests.Acceptance
{
    [TestFixture]
    public class AcceptanceTests
    {
        private ServerWrapper wrapper;

        [OneTimeSetUp]
        public void SetUp()
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            wrapper = ServerWrapper.Create<Startup>(TestContext.CurrentContext.TestDirectory, services => { });
        }

        [Test]
        public async Task Version()
        {
            var response = await wrapper.ApiClient.GetRequest<RawResponse<string>>("api/parser/version", CancellationToken.None).ConfigureAwait(false);
            Assert.IsTrue(response.IsSuccess);
        }

        [Test]
        public async Task Parse()
        {
            var parser = new DocumentParser(new ApiClientFactory(wrapper.Client, wrapper.Client.BaseAddress));
            var data = File.ReadAllBytes(Path.Combine(TestContext.CurrentContext.TestDirectory, "Data", "Research.pdf"));
            var request = new ParsingRequest();
            request.Name = "Test.pdf";
            request.Data = data;
            request.OnlyOcr = true;
            var result = await parser.Parse(request, CancellationToken.None).ConfigureAwait(false);
            Assert.AreEqual(35, result.Document.Pages.Length);
            Assert.GreaterOrEqual(result.Document.Pages[0].Build().Length, 1718);
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            wrapper.Dispose();
        }
    }
}

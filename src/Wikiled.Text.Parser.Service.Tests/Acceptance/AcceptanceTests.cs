using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Wikiled.Common.Net.Client;
using Wikiled.Server.Core.Testing.Server;
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
            DocumentParser parser = new DocumentParser(new ApiClientFactory(wrapper.Client, wrapper.Client.BaseAddress));
            var data = await File.ReadAllBytesAsync(Path.Combine(TestContext.CurrentContext.TestDirectory, "Data", "Research.pdf")).ConfigureAwait(false);
            var result = await parser.Parse("Test.pdf", data, CancellationToken.None).ConfigureAwait(false);
            Assert.AreEqual(99621, result.Text.Length);
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            wrapper.Dispose();
        }
    }
}

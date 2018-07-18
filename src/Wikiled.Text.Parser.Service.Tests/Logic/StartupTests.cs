using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Wikiled.Common.Net.Client;
using Wikiled.Server.Core.Testing.Server;

namespace Wikiled.Text.Parser.Service.Tests.Logic
{
    [TestFixture]
    public class StartupTests
    {
        private ServerWrapper wrapper;

        [SetUp]
        public void SetUp()
        {
            wrapper = ServerWrapper.Create<Startup>(TestContext.CurrentContext.TestDirectory, services => { });
        }

        [Test]
        public async Task Version()
        {
            var response = await wrapper.ApiClient.GetRequest<RawResponse<string>>("api/parser/version", CancellationToken.None).ConfigureAwait(false);
            Assert.IsTrue(response.IsSuccess);
        }

        [TearDown]
        public void Cleanup()
        {
            wrapper.Dispose();
        }
    }
}

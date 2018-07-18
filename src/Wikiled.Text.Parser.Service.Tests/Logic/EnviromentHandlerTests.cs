using System;
using Microsoft.AspNetCore.Hosting;
using Moq;
using NUnit.Framework;
using Wikiled.Common.Utilities.Config;
using Wikiled.Text.Parser.Service.Logic;

namespace Wikiled.Text.Parser.Service.Tests.Logic
{
    [TestFixture]
    public class EnviromentHandlerTests
    {
        private Mock<IApplicationConfiguration> mockApplicationConfiguration;

        private Mock<IHostingEnvironment> mockHostingEnvironment;

        private DocumentsConfig mockDocumentsConfig;

        private EnviromentHandler instance;

        [SetUp]
        public void SetUp()
        {
            mockApplicationConfiguration = new Mock<IApplicationConfiguration>();
            mockHostingEnvironment = new Mock<IHostingEnvironment>();
            mockHostingEnvironment.Setup(item => item.ContentRootPath).Returns(@"c:\test");
            mockDocumentsConfig = new DocumentsConfig();
            mockApplicationConfiguration.Setup(item => item.Now).Returns(new DateTime(2018, 01, 01));
            instance = CreateInstance();
        }

        [TestCase("File.txt", "documents", ExpectedResult = @"c:\test\documents\File_20180101120000.txt")]
        [TestCase("File.txt", @"c:\documents", ExpectedResult = @"c:\documents\File_20180101120000.txt")]
        public string GetFileName(string file, string save)
        {
            mockDocumentsConfig.Save = save;
            return instance.GetFileName(file);
        }

        [Test]
        public void Construct()
        {
            Assert.Throws<ArgumentNullException>(() => new EnviromentHandler(
                null,
                mockHostingEnvironment.Object,
                mockDocumentsConfig));
            Assert.Throws<ArgumentNullException>(() => new EnviromentHandler(
                mockApplicationConfiguration.Object,
                null,
                mockDocumentsConfig));
            Assert.Throws<ArgumentNullException>(() => new EnviromentHandler(
                mockApplicationConfiguration.Object,
                mockHostingEnvironment.Object,
                null));
        }

        private EnviromentHandler CreateInstance()
        {
            return new EnviromentHandler(
                mockApplicationConfiguration.Object,
                mockHostingEnvironment.Object,
                mockDocumentsConfig);
        }
    }
}

using System;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NUnit.Framework;
using Wikiled.Server.Core.Helpers;
using Wikiled.Text.Parser.Readers;
using Wikiled.Text.Parser.Service.Controllers;
using Wikiled.Text.Parser.Service.Logic;

namespace Wikiled.Text.Parser.Service.Tests.Controllers
{
    [TestFixture]
    public class ParserControllerTests
    {
        private ILogger<ParserController> logger;

        private Mock<IIpResolve> mockIpResolve;

        private Mock<ITextParserFactory> mockTextParserFactory;

        private Mock<DocumentsConfig> mockDocumentsConfig;

        private Mock<IHostingEnvironment> mockHostingEnvironment;

        private ParserController instance;

        [SetUp]
        public void SetUp()
        {
            logger = new NullLoggerFactory().CreateLogger<ParserController>();
            mockIpResolve = new Mock<IIpResolve>();
            mockTextParserFactory = new Mock<ITextParserFactory>();
            mockDocumentsConfig = new Mock<DocumentsConfig>();
            mockHostingEnvironment = new Mock<IHostingEnvironment>();
            instance = CreateParserController();
        }

        [Test]
        public void Construct()
        {
            Assert.Throws<ArgumentNullException>(() => new ParserController(
                null,
                mockIpResolve.Object,
                mockTextParserFactory.Object,
                mockDocumentsConfig.Object,
                mockHostingEnvironment.Object));
            Assert.Throws<ArgumentNullException>(() => new ParserController(
                logger,
                null,
                mockTextParserFactory.Object,
                mockDocumentsConfig.Object,
                mockHostingEnvironment.Object));
            Assert.Throws<ArgumentNullException>(() => new ParserController(
                logger,
                mockIpResolve.Object,
                null,
                mockDocumentsConfig.Object,
                mockHostingEnvironment.Object));
            Assert.Throws<ArgumentNullException>(() => new ParserController(
                logger,
                mockIpResolve.Object,
                mockTextParserFactory.Object,
                null,
                mockHostingEnvironment.Object));
            Assert.Throws<ArgumentNullException>(() => new ParserController(
                logger,
                mockIpResolve.Object,
                mockTextParserFactory.Object,
                mockDocumentsConfig.Object,
                null));
        }

        private ParserController CreateParserController()
        {
            return new ParserController(
                logger,
                mockIpResolve.Object,
                mockTextParserFactory.Object,
                mockDocumentsConfig.Object,
                mockHostingEnvironment.Object);
        }
    }
}
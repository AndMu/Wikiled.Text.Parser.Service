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
        private readonly ILoggerFactory loggerFactory = new NullLoggerFactory();

        private Mock<ITextParserFactory> mockTextParserFactory;

        private Mock<IEnviromentHandler> mockHandler;

        private ParserController instance;

        [SetUp]
        public void SetUp()
        {
            mockTextParserFactory = new Mock<ITextParserFactory>();
            mockHandler = new Mock<IEnviromentHandler>();
            mockTextParserFactory = new Mock<ITextParserFactory>();
            instance = CreateParserController();
        }

        [Test]
        public void Construct()
        {
            Assert.Throws<ArgumentNullException>(
                () => new ParserController(null,
                                           mockTextParserFactory.Object,
                                           mockHandler.Object));
            Assert.Throws<ArgumentNullException>(
                () => new ParserController(loggerFactory,
                                           null,
                                           mockHandler.Object));
            Assert.Throws<ArgumentNullException>(
                () => new ParserController(loggerFactory,
                                           mockTextParserFactory.Object,
                                           null));
        }

        private ParserController CreateParserController()
        {
            return new ParserController(loggerFactory,
                                        mockTextParserFactory.Object,
                                        mockHandler.Object);
        }
    }
}
using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Wikiled.Server.Core.ActionFilters;
using Wikiled.Server.Core.Controllers;
using Wikiled.Text.Parser.Api.Data;
using Wikiled.Text.Parser.Readers;
using Wikiled.Text.Parser.Service.Logic;

namespace Wikiled.Text.Parser.Service.Controllers
{
    [Route("api/[controller]")]
    [TypeFilter(typeof(RequestValidationAttribute))]
    public class ParserController : BaseController
    {
        private readonly ITextParserFactory parserFactory;

        private readonly IEnviromentHandler handler;

        public ParserController(ILoggerFactory loggerFactory,
                                ITextParserFactory parser,
                                IEnviromentHandler handler)
            : base(loggerFactory)
        {
            parserFactory = parser ?? throw new ArgumentNullException(nameof(parser));
            this.handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        [HttpPost, DisableRequestSizeLimit]
        [Route("processfile")]
        public ActionResult<ParsingResult> ProcessFile(IFormFile file)
        {
            if (file.Length != 1)
            {
                return StatusCode(500, "Only single file supported");
            }

            var fileNameData = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;
            string fileName = fileNameData.Value;
            string fullPath = handler.GetFileName(fileName);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            var fileInfo = new FileInfo(fullPath);
            var parser = parserFactory.ConstructParsers(fileInfo);
            var result = parser.Parse();
            ParsingResult parsingResult = new ParsingResult();
            parsingResult.Text = result;
            parsingResult.FileLength = fileNameData.Length;
            parsingResult.Name = fileInfo.Name;
            return Ok(parsingResult);
        }
    }
}

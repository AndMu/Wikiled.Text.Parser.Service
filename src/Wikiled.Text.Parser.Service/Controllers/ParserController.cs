using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Wikiled.Server.Core.ActionFilters;
using Wikiled.Server.Core.Controllers;
using Wikiled.Text.Parser.Api.Data;
using Wikiled.Text.Parser.Data;
using Wikiled.Text.Parser.Readers;
using Wikiled.Text.Parser.Readers.DevExpress;
using Wikiled.Text.Parser.Service.Logic;
using ParsingRequest = Wikiled.Text.Parser.Api.Data.ParsingRequest;
using ParsingResult = Wikiled.Text.Parser.Api.Data.ParsingResult;

namespace Wikiled.Text.Parser.Service.Controllers
{
    [Route("api/[controller]")]
    [TypeFilter(typeof(RequestValidationAttribute))]
    public class ParserController : BaseController
    {
        private readonly ITextParserFactory parserFactory;

        private readonly IEnviromentHandler handler;

        public ParserController(ILoggerFactory loggerFactory, ITextParserFactory parser, IEnviromentHandler handler)
            : base(loggerFactory)
        {
            parserFactory = parser ?? throw new ArgumentNullException(nameof(parser));
            this.handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        [HttpPost]
        [RequestSizeLimit(1024 * 1024 * 100)]
        [Route("processfile")]
        public async Task<ActionResult<ParsingResult>> ProcessFile([FromBody] ParsingRequest request)
        {
            string fullPath = handler.GetFileName(request.Name);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await stream.WriteAsync(request.Data, 0, request.Data.Length).ConfigureAwait(false);
            }

            var fileInfo = new FileInfo(fullPath);
            var parser = parserFactory.ConstructParsers(fileInfo);
            if (parser is NullTextParser)
            {
                return StatusCode(500, "Can't process this type of file");
            }

            ParsingType type;
            switch (request.Type)
            {
                case RequestParsingType.Extract:
                    type = ParsingType.Extract;
                    break;
                case RequestParsingType.OCR:
                    type = ParsingType.OCR;
                    break;
                case RequestParsingType.Any:
                    type = ParsingType.Any;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            var orcRequest = new Readers.ParsingRequest(fileInfo, type, 50);
            var result = await parser.Parse(orcRequest).ConfigureAwait(false);
            var parsingResult = new ParsingResult();
            parsingResult.Document = result.Document;
            parsingResult.Type = result.ProcessedAs?.ToString();
            parsingResult.Name = fileInfo.Name;
            return Ok(parsingResult);
        }
    }
}

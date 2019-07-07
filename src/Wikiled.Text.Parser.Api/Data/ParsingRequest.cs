using System.ComponentModel.DataAnnotations;

namespace Wikiled.Text.Parser.Api.Data
{
    public class ParsingRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public byte[] Data { get; set; }

        public bool OnlyOcr { get; set; }
    }
}

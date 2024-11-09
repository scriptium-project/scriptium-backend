using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using writings_backend_dotnet.Models;

namespace writings_backend_dotnet.DTOs
{
    public class RootSimpleDTO
    {
        public required string Latin { get; set; }
        public required string Own { get; set; }
    }

    public class RootExpandedDTO
    {
        public required string Latin { get; set; }
        public required string Own { get; set; }
        public List<WordRootDTO>? Words { get; set; } = [];
        public required short WordCount { get; set; } = -1;
    }

    public static class RootExtensions
    {
        public static RootSimpleDTO ToRootSimpleDTO(this Root root)
        {
            return new RootSimpleDTO
            {
                Latin = root.Latin,
                Own = root.Own,
            };
        }

        public static RootExpandedDTO ToRootExpandedDTO(this Root root)
        {
            return new RootExpandedDTO
            {
                Latin = root.Latin,
                Own = root.Own,
                Words = root.Words.Select(w => w.ToWordRootDTO()).ToList(),
                WordCount = root.WordCount
            };
        }
    }
}
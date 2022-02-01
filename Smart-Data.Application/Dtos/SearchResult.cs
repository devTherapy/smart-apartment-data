using System.Collections.Generic;

namespace Smart_Data.Application.Dtos
{
    public class SearchResult
    {
        public bool IsValid { get; set; }
        public long TotalResults { get; set; }
        public object Documents { get; set; }

    }
}

using MediatR;
using Smart_Data.Application.Responses;
using System.Collections.Generic;

namespace Smart_Data.Application.Dtos
{
    public class SearchQueryParams : IRequest<Response<SearchResult>>
    {
        public string Keyword { get; set; }
        public ICollection<string>Market { get; set; }
        public int Size { get; set; }
        public int PageNumber { get; set; }
    }
}

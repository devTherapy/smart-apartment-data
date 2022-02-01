using MediatR;
using Smart_Data.Application.Contracts;
using Smart_Data.Application.Dtos;
using Smart_Data.Application.Features.Query.GlobalSearch;
using Smart_Data.Application.Exceptions;
using Smart_Data.Application.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace Smart_Data.Application.Features.Query
{
    public class GlobalSearchHandler : IRequestHandler<SearchQueryParams, Response<SearchResult>>
    {
        private readonly IPropertySearchRepository _searchRepository;

        public GlobalSearchHandler(IPropertySearchRepository searchRepository)
        {
            _searchRepository = searchRepository;
        }
        public async Task<Response<SearchResult>> Handle(SearchQueryParams request, CancellationToken cancellationToken)
        {
            var validator = new GlobalSearchValidator();

            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid) throw new ValidationException(validationResult);

            var searchRequest = new SearchParams(request);

            var searchResponse =  await _searchRepository.Search(searchRequest.Keyword, searchRequest.Market, searchRequest.Size, searchRequest.Offset);

            return new Response<SearchResult>
            {
                Success = searchResponse.IsValid,
                Message = searchResponse.IsValid ? "query result returned successfully" : $"Unable to fetch query result for {searchRequest.Keyword}",
                Data = searchResponse
            };
            
        }
    }
}

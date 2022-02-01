
using Smart_Data.Application.Dtos;
using Smart_Data.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Smart_Data.Application.Contracts
{
    public interface ISearchRepository<T>
    {
        Task<bool> IndexExists(Indexes indexName);
        Task<SearchResult> Search(string keyword, ICollection<string> markets, int limit, int offset);
        Task<bool> Add(T data, Indexes IndexName);
        Task<bool> BulkAddAsync(List<T> data, Indexes indexName );

    }
}

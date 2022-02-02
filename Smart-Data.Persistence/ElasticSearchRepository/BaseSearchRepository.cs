using Nest;
using Smart_Data.Application.Contracts;
using Smart_Data.Application.Dtos;
using Smart_Data.Domain.Enums;
using Smart_Data.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aliases = Smart_Data.Domain.Enums.Aliases;
using Properties = Smart_Data.Domain.Models.Properties;

namespace Smart_Data.Persistence.ElasticSearchRepository
{
    public abstract class BaseSearchRepository<T> : ISearchRepository<T> where T : class
    {
        protected abstract string Index { get; }
        protected const string AutoCompleteAnalyzer = "AutoCompleteAnalyzer";
        protected const string KeywordAnalyzer = "KeywordAnalyzer";
        protected const string SearchAnalyzer = "SearchAnalyzer";

        protected readonly IElasticClient _client;

        public BaseSearchRepository(IElasticClient client)
        {
            _client = client;
        }
        public async Task<bool> Add(T data, Indexes indexName)
        {
            var index = await IndexExists(indexName) ? indexName.ToString() : Index;
            var response = await _client.IndexAsync(data, x => x.Index(index));
            return response.IsValid;
        }
        protected abstract Task<string> CreateIndex();
        public async Task<bool> IndexExists(Indexes indexName)
        {
            var res = await _client.Indices.ExistsAsync(indexName.ToString());
            return res.Exists;
        }
        public async  Task<bool> BulkAddAsync(List<T> data, Indexes indexName)
        {
            var index = await IndexExists(indexName) ? indexName.ToString() : Index;
            var res = await _client.IndexManyAsync(data, index);
            return res.IsValid;
        }
        public async Task<SearchResult> Search(string keyword, ICollection<string> markets, int limit, int offset)
        {
            var alias = Aliases.entities.ToString();
            //create management fields with boosts
            var managementName = Infer.Field<Managements>(ff => ff.Management.Name, 1.7);
            var managementState = Infer.Field<Managements>(ff => ff.Management.State, 1.5);
            var managementMarket = Infer.Field<Managements>(ff => ff.Management.Market);
            var managementExactName = Infer.Field<Managements>(ff => ff.Management.Name.Suffix("exact"), 1.8);

            //create property fields with boost
            var propertyName = Infer.Field<Properties>(ff => ff.Property.Name, 1.7);
            var propertyState = Infer.Field<Properties>(ff => ff.Property.State, 1.5);
            var propertyMarket = Infer.Field<Properties>(ff => ff.Property.Market, 1.4);
            var propertyFormerName = Infer.Field<Properties>(ff => ff.Property.FormerName, 1.6);
            var propertyCity = Infer.Field<Properties>(ff => ff.Property.City, 1.5);
            var exactPropertyName = Infer.Field<Properties>(ff => ff.Property.Name.Suffix("exact"), 1.8);
            var propertyStreetAddress = Infer.Field<Properties>(ff => ff.Property.StreetAddress, 1.4);
         
            var searchResult = await _client.SearchAsync<object>(s =>
            s.Index(alias)
             .From(offset)
             .Size(limit)
             .Query(p =>
            (
            +p.Terms(t => t.Field("management.market.keyword").Terms(markets))
            &&
            p.MultiMatch(m => m
                .Fields(f => f
                    .Field(managementState)
                    .Field(managementName)
                    .Field(managementMarket)
                    .Field(managementExactName)
                        )
                .Operator(Operator.Or)
                .Query(keyword)
                         ) 
              ||
                (
                +p.Terms(t => t.Field("property.market.keyword").Terms(markets))
                &&
                p
                .MultiMatch(m => m
                .Fields(f => f
                    .Field(propertyName)
                    .Field(propertyFormerName)
                    .Field(propertyCity)
                    .Field(propertyState)
                    .Field(propertyMarket)
                    .Field(exactPropertyName)
                    .Field(propertyStreetAddress)
                        )
                .Operator(Operator.Or)
                .Query(keyword)
                            )
                ))));

            var xx = searchResult.DebugInformation;

            return new SearchResult { IsValid = searchResult.IsValid, TotalResults = searchResult.Total, Documents = searchResult.Documents};
        }
        protected IAnalysis CommonAnalyzer(AnalysisDescriptor analysis)
        {
            return analysis
                 .Analyzers(an => an
                     .Custom(AutoCompleteAnalyzer, ca => ca
                         .CharFilters("html_strip")
                         .Tokenizer("autocomplete")
                         .Filters("lowercase", "stop", "eng_stopwords", "trim")
                             )
                     .Custom(KeywordAnalyzer, ca => ca
                         .CharFilters("html_strip")
                         .Tokenizer("keyword")
                         .Filters("lowercase", "eng_stopwords", "trim")
                             )
                     .Custom(SearchAnalyzer, ca => ca
                         .Filters("eng_stopwords", "trim")
                         .Tokenizer("lowercase")
                             )
                             )
                 .Tokenizers(t => t
                     .EdgeNGram("autocomplete", ed => ed
                         .MinGram(2)
                         .MaxGram(20)
                         .TokenChars(TokenChar.Letter)
                                 )
                             )
                .TokenFilters(f => f
                    .Stop("eng_stopwords", lang => lang
                        .StopWords("_english_")
                    )
                );
        }
      
    }
}

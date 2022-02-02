using Nest;
using Smart_Data.Application.Contracts;
using Smart_Data.Domain.Enums;
using Smart_Data.Domain.Models;
using System.Threading.Tasks;
using Aliases = Smart_Data.Domain.Enums.Aliases;

namespace Smart_Data.Persistence.ElasticSearchRepository
{
    public class ManagementSearchRepository : BaseSearchRepository<Managements>, IManagementSearchRepository
    {
        protected  override string Index  => CreateIndex().Result;

        public ManagementSearchRepository(IElasticClient client) : base(client)
        {

        }

        protected async override Task<string> CreateIndex()
        {
            var index = Indexes.managements.ToString();
            var alias = Aliases.entities.ToString();
            var createIndexResponse = await _client.Indices.CreateAsync(index, c => c
                .Settings(s => s
                    .Analysis(CommonAnalyzer)
                    )
                .Aliases(s => s.Alias(alias))
                .Map<ManagementData>(m => m
                    .Properties(p => p
                        .Text(t => t
                            .Name(n => n.Name)
                            .Analyzer(AutoCompleteAnalyzer)
                            .Fields(t => t
                        .Text(t => t
                            .Name("exact")
                            .Analyzer(KeywordAnalyzer)
                            )
                            )
                            .SearchAnalyzer(SearchAnalyzer)
                            )
                         .Text(t => t
                            .Name(n => n.State)
                            .Analyzer(AutoCompleteAnalyzer)
                            .SearchAnalyzer(SearchAnalyzer))
                        .Keyword(k => k
                            .Name(n => n.Market)
                                )
                                 )
                                   )
                );
            return createIndexResponse.Index;
        }

       

    }
}

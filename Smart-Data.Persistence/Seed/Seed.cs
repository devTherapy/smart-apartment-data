using Smart_Data.Application.Contracts;
using Smart_Data.Application.Helper;
using Smart_Data.Domain.Enums;
using Smart_Data.Domain.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Smart_Data.Persistence.Seed
{
    public static class Seed
    {
        public async static void LoadData(IPropertySearchRepository propertyRepo, IManagementSearchRepository managementRepo)
        {

            if (!await managementRepo.IndexExists(Indexes.managements))
            {
                var managements = Deserialize<Managements>("mgmt.json");
                if (managements.Any())
                {
                    await managementRepo.BulkAddAsync(managements, Indexes.managements);
                }
            }

            if (!await propertyRepo.IndexExists(Indexes.properties))
            {
                var properties = Deserialize<Properties>("properties.json");
                if (properties.Any())
                {
                    await propertyRepo.BulkAddAsync(properties, Indexes.properties);
                }
            }


        }

        private static List<T> Deserialize<T>(string json)
        {
            var path = Path.GetFullPath(@"../Smart-Data.Persistence/Seed/" + json);
            return File.Exists(path) ? File.ReadAllText(path).Deserialize<List<T>>() : new List<T>();
        }
    }
}

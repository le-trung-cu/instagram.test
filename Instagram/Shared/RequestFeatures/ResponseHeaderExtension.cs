using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Shared.RequestFeatures
{
    public static class ResponseHeaderExtension
    {
        public static void AddXPagination(this IHeaderDictionary header, MetaData metaData)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            header.Add("X-Pagination", JsonSerializer.Serialize(metaData, options));
        }
    }
}

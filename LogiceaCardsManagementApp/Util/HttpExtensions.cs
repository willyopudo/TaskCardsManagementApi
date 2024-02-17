using LogiceaCardsManagementApp2.Util.Pagination;
using System.Text.Json;

namespace LogiceaCardsManagementApp2.Util
{
    public static class HttpExtensions
    {
        public static void AddPaginationHeader(this HttpResponse response, MetaData metadata)
        {
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            response.Headers.Add("Pagination", JsonSerializer.Serialize(metadata, options));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}

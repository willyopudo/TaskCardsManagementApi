using LogiceaCardsManagementApp2.Util.Pagination;

namespace LogiceaCardsManagementApp2.Util.Filter
{
    public class SearchParams : PaginationParams
    {
        public string? OrderBy { get; set; }
        public string? SearchTerm { get; set; }
        public string? ColumnFilters { get; set; }
    }
}

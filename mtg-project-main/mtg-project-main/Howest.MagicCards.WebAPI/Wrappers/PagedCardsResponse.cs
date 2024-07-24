using Howest.MagicCards.Shared.Wrappers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Howest.MagicCards.WebAPI.Wrappers
{
    public class PagedCardsResponse<T>: CardsResponse<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }

        public PagedCardsResponse(T data, int pageNumber, int pageSize, int totalRecords)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalRecords = totalRecords;
            TotalPages = (int)Math.Ceiling((double)TotalRecords / PageSize);
            Data = data;
        }
    }
}

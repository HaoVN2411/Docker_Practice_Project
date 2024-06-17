namespace NET1717_Lab01_ProductManagement.API.Models
{
    public class PagedResponse<T> : BaseResponse<T>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public PagedResponse():base()
        {

        }
        public PagedResponse(int status, string message, T data, int pageIndex
            , int pageSize, int totalPages, int totalCount)
            :base(status, message, data)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalPages = totalPages;
            TotalCount = totalCount;
        }
    }
}

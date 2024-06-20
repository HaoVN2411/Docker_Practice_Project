namespace NET1717_Lab01_ProductManagement.API.Models
{
    public class PagedResponse <T>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public T Data { get; set; }

        public PagedResponse()
        {
            
        }
    }
}

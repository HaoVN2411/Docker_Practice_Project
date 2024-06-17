namespace NET1717_Lab01_ProductManagement.API.Models
{
    public class BaseResponse<T>
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public BaseResponse(int status, string message, T data)
        {
            Status = status;
            Message = message;
            Data = data;
        }
        public BaseResponse()
        {
            
        }
    }
}

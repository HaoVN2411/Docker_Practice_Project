namespace NET1717_Lab01_ProductManagement.API.Middleware
{
    public class BaseResponseMiddleware<T>
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }
        public BaseResponseMiddleware(int status, string message, T data)
        {
            Status = status;
            Message = message;
            Data = data;
        }
        public BaseResponseMiddleware()
        {
        }
    }
}

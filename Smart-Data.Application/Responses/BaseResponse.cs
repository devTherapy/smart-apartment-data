
namespace Smart_Data.Application.Responses
{
    public class Response<T> where T : class
    {
        public Response()
        {
            Success = false;
        }
        public Response(string message = null)
        {
            Success = false;
            Message = message;
        }

        public Response(string message, bool success)
        {
            Success = success;
            Message = message;
        }

        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}

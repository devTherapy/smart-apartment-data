using System.Collections.Generic;

namespace Smart_Data.Application.Responses
{
    public class ApiException
    {
        public ApiException()
        {
            Success = false;
        }
        
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<string> ValidationErrors { get; set; }
    }
}

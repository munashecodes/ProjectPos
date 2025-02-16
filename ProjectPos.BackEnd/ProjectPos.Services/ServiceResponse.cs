using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services
{
    public class ServiceResponse<T>
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public DateTime Time { get; set; }
        public T? Data { get; set; }
        
        public static ServiceResponse<T> Success(T data, string? message = null)
        {
            return new ServiceResponse<T>
            {
                Data = data,
                IsSuccess = true,
                Message = message ?? "Operation completed successfully",
                Time = DateTime.UtcNow
            };
        }

        public static ServiceResponse<T> Failure(string message)
        {
            return new ServiceResponse<T>
            {
                IsSuccess = false,
                Message = message,
                Time = DateTime.UtcNow
            };
        }
    }
}

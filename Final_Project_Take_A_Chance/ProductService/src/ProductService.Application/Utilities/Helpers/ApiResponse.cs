using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProductService.Application.Utilities.Helpers
{
    public abstract class Result
    {
        public string Message { get; set; }
        public bool Success { get; protected set; }
        public int StatusCode { get; protected set; }
    }

    public abstract class Result<T> : Result
    {
        private T _entities;

        protected Result(T data)
        {
            Entities = data;
        }

        public T Entities
        {
            get => _entities;
            set => _entities = value;
        }
    }

    public class SuccessResult : Result
    {
        public SuccessResult(string message, int statusCode)
        {
            Success = true;
            Message = message;
            StatusCode = statusCode;
        }
    }

    public class SuccessResult<T> : Result<T>
    {
        public SuccessResult(T data, string message, int statusCode) : base(data)
        {
            Success = true;
            Message = message;
            StatusCode = statusCode;
        }
    }

    public class ErrorResult : Result
    {
        public ErrorResult(string message, int statusCode, string propertyName = "")
        {
            Message = message;
            Success = false;
            StatusCode = statusCode;
            PropertyName = propertyName;
        }
        public string PropertyName { get; set; }
    }

    public class ErrorResult<T> : Result<T>
    {
        public ErrorResult(string message, int statusCode, string propertyName = "") : base(default)
        {
            Message = message;
            Success = false;
            StatusCode = statusCode;
            PropertyName = propertyName;
        }
        public string PropertyName { get; set; }

    }


}

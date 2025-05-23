﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LotteryService.Application.Utilities.Helpers
{
    public class Result
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string PropertyName { get; set; }
    }

    public class Result<T> : Result
    {
        public T Entities { get; set; }
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
        public SuccessResult(T data, string message, int statusCode)
        {
            Entities = data;
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
    }

    public class ErrorResult<T> : Result<T>
    {
        public ErrorResult(string message, int statusCode, string propertyName = "")
        {
            Message = message;
            Success = false;
            StatusCode = statusCode;
            PropertyName = propertyName;
        }
    }


}

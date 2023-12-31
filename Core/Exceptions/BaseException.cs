using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Exceptions
{
    public class BaseException : Exception
    {
        public ErrorCode ErrorCode;
        public BaseException(string message, ErrorCode errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }
    }

    public enum ErrorCode {
        InternalServerError = 0,
        BusinessError = 1
    }
}

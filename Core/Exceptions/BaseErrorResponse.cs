namespace Core.Exceptions
{
    public class BaseErrorResponse
    {
        public ErrorCode ErrorCode { get; }
        public int StatusCode { get; }
        public string ErrorMessage { get; }

        public BaseErrorResponse(ErrorCode errorCode, string errorMessage, int statusCode)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
            StatusCode = statusCode;
        }
    }
}

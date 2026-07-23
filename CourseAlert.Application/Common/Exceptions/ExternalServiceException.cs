namespace CourseAlert.Application.Common.Exceptions;

public sealed class ExternalServiceException : Exception
{
    public ExternalServiceException(string message)
        : base(message)
    {
    }
}

using System.Net;

namespace Portfolio.WebApi.Errors;

public class RequestException : Exception
{
  public int Code { get; }
  public IEnumerable<string> ErrorMessages { get; }

  public RequestException(int code)
  {
    Code = code;
    ErrorMessages = new List<string> { ((HttpStatusCode)code).ToString() };
  }

  public RequestException(int code, IEnumerable<string> errorMessages)
  {
    Code = code;
    ErrorMessages = errorMessages;
  }

  public RequestException(int code, string message)
  {
    Code = code;
    ErrorMessages = new List<string> { message };
  }
}

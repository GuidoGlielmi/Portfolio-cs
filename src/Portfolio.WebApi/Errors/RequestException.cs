using Portfolio.WebApi.DTO;

namespace Portfolio.WebApi.Errors;

public class RequestException : Exception
{
  private enum StatusMessages
  {
    Server_Error = 500,
    Not_Found = 404,
    Bad_Request = 400,
    Forbidden = 403, // used for an existent user that wants to access protected info
  }

  public ResponseDto<string> Error { get; set; }

  public RequestException(int code)
  {
    Error = new ResponseDto<string>(code, ((StatusMessages)code).ToString());
  }

  public RequestException(int code, IEnumerable<string> messages)
  {
    Error = new ResponseDto<string>(code, messages);
  }

  public RequestException(int code, string message)
      : base(message)
  {
    Error = new ResponseDto<string>(code, message);
  }
}

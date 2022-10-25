namespace Portfolio.WebApi.DTO;

public class ResponseDto<T>
{

  public IEnumerable<T> Data { get; set; }

  public int Code { get; set; }

  public IEnumerable<string> Messages { get; set; } = new List<string> { "Operation successful" };

  public ResponseDto(int code = 200)
  {
    Code = code;
    if (code is < 200 or >= 300)
    {
      Messages = new string[] { "Operation failed" };
    }
  }

  public ResponseDto(int code, string message)
  {
    Messages = new List<string> { message };
    Code = code;
  }

  public ResponseDto(int code, IEnumerable<string> message)
  {
    Messages = message;
    Code = code;
  }

  public ResponseDto(IEnumerable<T> data, int code = 200)
  {
    Data = data;
    Code = code;
  }
  public ResponseDto(T data, int code = 200)
  {
    Data = new T[] { data };
    Code = code;
  }

  public ResponseDto(T data, string message, int code = 200)
  {
    Data = new T[] { data };
    Code = code;
    Messages = new List<string> { message };
  }

  public ResponseDto(T data, IEnumerable<string> messages, int code = 200)
  {
    Data = new T[] { data };
    Code = code;
    Messages = messages;
  }
}

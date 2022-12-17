using FluentValidation;
using MediatR;
using Portfolio.WebApi.Errors;
using Portfolio.WebApi.Mediator.Commands;

namespace Portfolio.WebApi.PipelineBehaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
  where TRequest : ICommand<TResponse>
  // the TRequest and TResponse signatures must be honored to apply the pipeline behavior
{
  // called after CreateEducationCommandValidator
  // comes before the handler
  private readonly IEnumerable<IValidator<TRequest>> _validators;
  public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) // injected by .AddValidatorsFromAssembly()
  {
    // a list of FluentValidators applied strictly to commands
    _validators = validators;
  }

  public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
  {
    // the request is the command or query with their arguments
    var failures = _validators
     .Select(v => v.Validate(request)) // TRequest is what an ICommand<TResponse> returns
     .SelectMany(v => v.Errors)
     .Where(e => e != null)
     .Select(e => e.ErrorMessage);
    if (failures.Any())
    {
      throw new RequestException(400, failures);
    }
    // All the processing of _validators is not done until Any() is called,
    // because it's not being accessed previously

    return await next();
  }

}

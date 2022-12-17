using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Portfolio.WebApi.DTO.EducationDtos;
using Portfolio.WebApi.Errors;
using Portfolio.WebApi.Extensions;
using Portfolio.WebApi.Mediator.Commands.EducationCommands;
using Portfolio.WebApi.Mediator.Queries.EducationQueries;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Mediator.Handlers.EducationHandlers;

public class PatchEducationHandler : IRequestHandler<PatchEducationCommand>
{
  private readonly IMediator _mediator;

  public PatchEducationHandler(IMediator mediator)
  {
    _mediator = mediator;
  }
  public async Task<Unit> Handle(PatchEducationCommand request, CancellationToken cancellationToken)
  {
    // a patch request should be handled through content negotiation,
    // which make it possible to serve different versions of a resource at the same URI
    // indicated by the user.
    /*
    A JsonPatchDocument has this structure:
    [
      {
        "op": <action-to-perform>, // add - remove - replace - move - copy - test
        "path": <prop-to-modify>,
        "value": <entered-value>
      }
    ]
    which is a list of object operations to perform on an entity
    */
    EducationPutDto foundEd2 = await _mediator.Send(new GetEducationByIdQuery(request.Id), cancellationToken);
    request.PatchDocument.ApplyTo(foundEd2);
    await _mediator.Send(new PutEducationCommand(foundEd2), cancellationToken);
    return Unit.Value;
  }
}
//[
//  {
//    "path": "/Degree",
//    "op": "replace",
//    "value": "asdasdasdasd"
//  },
//]

//Education foundEd = await _context.Educations.FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
//if(foundEd == null)
//{
//  throw new RequestException(404, "Education not found");
//}
//EducationPutDto edDto = _mapper.Map<EducationPutDto>(foundEd);
//request.PatchDocument.ApplyTo(edDto);
//if (!edDto.Validate(out var validationResults))
//{
//  throw new RequestException(400, validationResults);
//}
//try
//{
//  edDto.SetTo(foundEd);
//} catch (Exception e)
//{
//  throw new RequestException(500, e.Message);
//}
//await _context.SaveChangesAsync(cancellationToken);
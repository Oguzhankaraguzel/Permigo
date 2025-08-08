using MediatR;
using SharedKernel.Concrete;

namespace Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;

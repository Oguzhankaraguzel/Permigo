using MediatR;
using SharedKernel.Concrete;

namespace Application.Abstractions.Messaging;

public interface IBaseCommand;

public interface ICommand : IRequest<Result>, IBaseCommand;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand;

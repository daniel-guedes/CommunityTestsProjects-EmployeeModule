using MediatR;

namespace Funcionarios.Application.Commands.DeleteFuncionarioCommand;

public record DeleteFuncionarioCommandInput(
	Guid Id
	) : IRequest<bool>;

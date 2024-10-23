using Funcionarios.Domain.EmployeeAggregate;
using MediatR;

namespace Funcionarios.Application.Commands.UpdateFuncionarioCommand;

public record UpdateFuncionarioCommandInput(
	Guid Id,
	string NomeFuncionario,
	string Email,
	string Cargo,
	DateTime DataNascimento,
	string Login,
	string Senha,
	SituacaoFuncionario Situacao
	) : IRequest<bool>;

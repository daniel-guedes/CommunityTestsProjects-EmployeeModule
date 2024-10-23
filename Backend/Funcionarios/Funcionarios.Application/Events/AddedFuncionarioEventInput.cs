using Funcionarios.Domain.EmployeeAggregate;
using MediatR;

namespace Funcionarios.Application.Events;

public record AddedFuncionarioEventInput(
	Guid Id, 
	string NomeFuncionario, 
	string Email,
	string Cargo,
	DateTime DataNascimento,
	string Login,
	string Senha,
	SituacaoFuncionario Situacao
	) : INotification;

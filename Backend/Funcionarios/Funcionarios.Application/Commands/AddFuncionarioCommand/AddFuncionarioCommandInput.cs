using Funcionarios.Domain.EmployeeAggregate;
using MediatR;

namespace Funcionarios.Application.Commands.AddFuncionarioCommand;

public record AddFuncionarioCommandInput(
    string NomeFuncionario,
    string Email,
    string Cargo,
    DateTime DataNascimento,
    string Login,
    string Senha,
    SituacaoFuncionario Situacao
    ) : IRequest<Guid>;

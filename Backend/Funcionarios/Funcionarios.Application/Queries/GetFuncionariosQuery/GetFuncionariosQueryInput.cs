using Funcionarios.Domain.EmployeeAggregate;
using MediatR;

namespace Funcionarios.Application.Queries.GetFuncionariosQuery;

public record GetFuncionariosQueryInput : IRequest<List<FuncionarioModel>>;

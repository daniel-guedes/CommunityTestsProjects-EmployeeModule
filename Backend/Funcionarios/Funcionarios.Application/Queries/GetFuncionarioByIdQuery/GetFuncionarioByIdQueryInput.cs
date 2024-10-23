using Funcionarios.Domain.EmployeeAggregate;
using MediatR;

namespace Funcionarios.Application.Queries.GetFuncionarioByIdQuery;

public record GetFuncionarioByIdQueryInput(Guid id) : IRequest<FuncionarioModel>;
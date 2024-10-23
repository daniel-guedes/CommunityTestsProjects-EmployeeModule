using MediatR;

namespace Funcionarios.Application.Events;

public record UpdatedFuncionarioEventInput(Guid Id, string NomeFucionario, string Cargo) : INotification;


using MediatR;

namespace Funcionarios.Application.Events;

public class DeletedFuncionarioEventInput(Guid Id) : INotification;

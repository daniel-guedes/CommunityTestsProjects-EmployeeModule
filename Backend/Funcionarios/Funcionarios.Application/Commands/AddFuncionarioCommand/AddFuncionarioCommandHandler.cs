using Funcionarios.Application.Events;
using Funcionarios.Domain.EmployeeAggregate;
using Funcionarios.Infra.Repositories.FuncionariosRepository;
using MediatR;

namespace Funcionarios.Application.Commands.AddFuncionarioCommand;

public class AddFuncionarioCommandHandler(IFuncionariosRepository funcionariosRepository, IMediator mediator) : IRequestHandler<AddFuncionarioCommandInput, Guid>
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    private readonly IFuncionariosRepository _funcionariosRepository = funcionariosRepository ?? throw new ArgumentNullException(nameof(funcionariosRepository));

    public async Task<Guid> Handle(AddFuncionarioCommandInput request, CancellationToken cancellationToken)
    {
        var funcionario = new Funcionario(
            request.NomeFuncionario,
            request.Cargo,
            request.Email,
            request.DataNascimento,
            request.Login,
            request.Senha,
            request.Situacao
            );
        await _funcionariosRepository.AddAsync(funcionario, cancellationToken);
        await _funcionariosRepository.UnitOfWork.CommitAsync(cancellationToken);

        await _mediator.Publish(new AddedFuncionarioEventInput(
            funcionario.Id,
            funcionario.NomeFuncionario,
            funcionario.Cargo,
            funcionario.Email,
            funcionario.DataNascimento,
            funcionario.Login,
            funcionario.Senha,
            funcionario.Situacao
            ), cancellationToken);

        return funcionario.Id;
    }
}

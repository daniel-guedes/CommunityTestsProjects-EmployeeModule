using Funcionarios.Application.Events;
using Funcionarios.Infra.Repositories.FuncionariosRepository;
using MediatR;

namespace Funcionarios.Application.Commands.DeleteFuncionarioCommand;

public class DeleteFuncionarioCommandHandler(IFuncionariosRepository funcionariosRepository, IMediator mediator) : IRequestHandler<DeleteFuncionarioCommandInput, bool>
{
	private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
	private readonly IFuncionariosRepository _funcionariosRepository = funcionariosRepository ?? throw new ArgumentNullException(nameof(funcionariosRepository));

	public async Task<bool> Handle(DeleteFuncionarioCommandInput request, CancellationToken cancellationToken)
	{
		var funcionario = await _funcionariosRepository.GetAsync(g => g.Id == request.Id, cancellationToken);

		if (funcionario is null)
			return false;

		_funcionariosRepository.Remove(funcionario);
		await _funcionariosRepository.UnitOfWork.CommitAsync(cancellationToken);

		await _mediator.Publish(new DeletedFuncionarioEventInput(funcionario.Id), cancellationToken);

		return true;
	}
}
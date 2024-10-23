using Funcionarios.Application.Events;
using Funcionarios.Infra.Repositories.FuncionariosRepository;
using MediatR;

namespace Funcionarios.Application.Commands.UpdateFuncionarioCommand;

public class UpdateFuncionarioCommandHandler(IFuncionariosRepository funcionariosRepository,
	IMediator mediator) : IRequestHandler<UpdateFuncionarioCommandInput, bool>
{

	private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
	private readonly IFuncionariosRepository _funcionariosRepository = funcionariosRepository ?? throw new ArgumentNullException(nameof(funcionariosRepository));

	public async Task<bool> Handle(UpdateFuncionarioCommandInput request, CancellationToken cancellationToken)
	{
		var funcionario = await _funcionariosRepository.GetAsync(g => g.Id == request.Id, cancellationToken);

		if (funcionario is null)
			return false;

		funcionario.WithNomeFuncionario(request.NomeFuncionario);
		funcionario.WithCargo(request.Cargo);
		funcionario.WithEmail(request.Email);
		funcionario.WithDataNascimento(request.DataNascimento);
		funcionario.WithLogin(request.Login);
		funcionario.WithSenha(request.Senha);
		funcionario.WithSituacao(request.Situacao);

		_funcionariosRepository.Update(funcionario);
		await _funcionariosRepository.UnitOfWork.CommitAsync(cancellationToken);

		await _mediator.Publish(new UpdatedFuncionarioEventInput(funcionario.Id, funcionario.NomeFuncionario, funcionario.Cargo), cancellationToken);

		return true;
	}
}
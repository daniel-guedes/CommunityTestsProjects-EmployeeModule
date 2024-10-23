using Funcionarios.Application.Events;
using Funcionarios.Domain.EmployeeAggregate;
using Funcionarios.Infra.Repositories.FuncionariosRepository;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace Funcionarios.Application.Subscribers;

public class AddedFuncionarioSubscriber(
	ILogger<AddedFuncionarioSubscriber> logger,
	IFuncionarioMongoRepository funcionariosRepository
	) : INotificationHandler<AddedFuncionarioEventInput>
{
	private readonly IFuncionarioMongoRepository _funcionariosRepository = funcionariosRepository ?? throw new ArgumentNullException(nameof(funcionariosRepository));
	private readonly ILogger<AddedFuncionarioSubscriber> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

	public async Task Handle(AddedFuncionarioEventInput notification, CancellationToken cancellationToken)
	{
		try
		{
			var funcionarioReadModel = new FuncionarioModel
			{
				Id = notification.Id,
				NomeFuncionario = notification.NomeFuncionario,
				Cargo = notification.Cargo,
				DataNascimento = notification.DataNascimento,
				Email = notification.Email,
				Login = notification.Login,
				Senha = notification.Senha,
				Situacao = notification.Situacao.ToString()
			};

			var success = await _funcionariosRepository.InsertAsync(funcionarioReadModel, cancellationToken);
			if (!success)
				throw new Exception("Erro ao inserir funcionario no MongoDB.");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Erro ao processar AddFuncionarioEventInput.");
		}
	}
}

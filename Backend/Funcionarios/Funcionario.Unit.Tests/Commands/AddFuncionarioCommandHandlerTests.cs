using Funcionarios.Application.Commands.AddFuncionarioCommand;
using Funcionarios.Application.Events;
using Funcionarios.Domain.Data;
using Funcionarios.Domain.EmployeeAggregate;
using Funcionarios.Infra.Repositories.FuncionariosRepository;
using MediatR;
using Moq;

namespace Funcionarios.Unit.Tests.Commands
{
	public class AddFuncionarioCommandHandlerTests
	{
		private readonly Mock<IFuncionariosRepository> _funcionariosRepositoryMock;
		private readonly Mock<IMediator> _mediatorMock;
		private readonly AddFuncionarioCommandHandler _handler;
		private readonly Mock<IUnitOfWork> _unitOfWorkMock;

		public AddFuncionarioCommandHandlerTests()
		{
			_funcionariosRepositoryMock = new Mock<IFuncionariosRepository>();
			_mediatorMock = new Mock<IMediator>();
			_unitOfWorkMock = new Mock<IUnitOfWork>();
			_funcionariosRepositoryMock.Setup(r => r.UnitOfWork).Returns(_unitOfWorkMock.Object);
			_handler = new AddFuncionarioCommandHandler(_funcionariosRepositoryMock.Object, _mediatorMock.Object);
		}

		[Fact]
		public async Task Handle_ShouldAddFuncionarioAndPublishEvent()
		{
			// Arrange
			var cancellationToken = new CancellationToken();
			var command = new AddFuncionarioCommandInput("Usuario Joao", "teste_joao@joaomail.com.br", "Analista de Dados", DateTime.Now, "joaotest", "a3J-@1bCd", SituacaoFuncionario.Ativo);

			// Act
			await _handler.Handle(command, cancellationToken);

			// Assert
			_funcionariosRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Funcionario>(), cancellationToken), Times.Once);
			_funcionariosRepositoryMock.Verify(x => x.UnitOfWork.CommitAsync(cancellationToken), Times.Once);
			_mediatorMock.Verify(x => x.Publish(It.IsAny<AddedFuncionarioEventInput>(), cancellationToken), Times.Once);
		}
	}
}

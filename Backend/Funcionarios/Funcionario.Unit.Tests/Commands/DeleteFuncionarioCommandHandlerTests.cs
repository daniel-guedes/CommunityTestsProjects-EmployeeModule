using Funcionarios.Application.Commands.DeleteFuncionarioCommand;
using Funcionarios.Application.Events;
using Funcionarios.Domain.Data;
using Funcionarios.Domain.EmployeeAggregate;
using Funcionarios.Infra.Repositories.FuncionariosRepository;
using MediatR;
using Moq;
using System.Linq.Expressions;


namespace Funcionarios.Unit.Tests.Commands
{
	public class DeleteFuncionarioCommandHandlerTests
	{
		private readonly Mock<IFuncionariosRepository> _funcionariosRepositoryMock;
		private readonly Mock<IMediator> _mediatorMock;
		private readonly DeleteFuncionarioCommandHandler _handler;
		private readonly Mock<IUnitOfWork> _unitOfWorkMock;

		public DeleteFuncionarioCommandHandlerTests()
		{
			_funcionariosRepositoryMock = new Mock<IFuncionariosRepository>();
			_mediatorMock = new Mock<IMediator>();
			_unitOfWorkMock = new Mock<IUnitOfWork>();
			_funcionariosRepositoryMock.Setup(r => r.UnitOfWork).Returns(_unitOfWorkMock.Object);
			_handler = new DeleteFuncionarioCommandHandler(_funcionariosRepositoryMock.Object, _mediatorMock.Object);
		}

		[Fact]
		public async Task Handle_ShouldUpdateFuncionarioAndPublishEvent()
		{
			// Arrange
			var cancellationToken = new CancellationToken();
			var funcionario = new Funcionario("Usuario Jose", "jose_test@josemail.com.br", "Diretor de Finanças", DateTime.Parse("1995-11-24"), "josetest", "1aF2@+e4k", SituacaoFuncionario.Ativo);
			var command = new DeleteFuncionarioCommandInput(funcionario.Id);

			var funcionariosQueryable = new[] { funcionario }.AsQueryable();

			_funcionariosRepositoryMock
				   .Setup(r => r.GetAsync(It.IsAny<Expression<Func<Funcionario, bool>>>(), It.IsAny<CancellationToken>()))
				   .Returns((Expression<Func<Funcionario, bool>> predicate, CancellationToken ct) =>
					   Task.FromResult(funcionariosQueryable.FirstOrDefault(predicate)));

			// Act
			var result = await _handler.Handle(command, cancellationToken);

			// Assert
			_funcionariosRepositoryMock.Verify(x => x.Remove(It.IsAny<Funcionario>()), Times.Once);
			_unitOfWorkMock.Verify(x => x.CommitAsync(cancellationToken), Times.Once);
			_mediatorMock.Verify(x => x.Publish(It.IsAny<DeletedFuncionarioEventInput>(), cancellationToken), Times.Once);
			Assert.True(result);
		}
	}
}

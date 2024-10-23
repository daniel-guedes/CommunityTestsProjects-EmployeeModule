using Funcionarios.Application.Commands.AddFuncionarioCommand;
using Funcionarios.Application.Commands.UpdateFuncionarioCommand;
using Funcionarios.Application.Events;
using Funcionarios.Domain.Data;
using Funcionarios.Domain.EmployeeAggregate;
using Funcionarios.Infra.Repositories.FuncionariosRepository;
using MediatR;
using Moq;
using System.Linq.Expressions;

namespace Funcionarios.Unit.Tests.Commands
{
	public class UpdateFuncionarioCommandHandlerTests
	{
		private readonly Mock<IFuncionariosRepository> _funcionariosRepositoryMock;
		private readonly Mock<IMediator> _mediatorMock;
		private readonly UpdateFuncionarioCommandHandler _handler;
		private readonly Mock<IUnitOfWork> _unitOfWorkMock;

		public UpdateFuncionarioCommandHandlerTests()
		{
			_funcionariosRepositoryMock = new Mock<IFuncionariosRepository>();
			_mediatorMock = new Mock<IMediator>();
			_unitOfWorkMock = new Mock<IUnitOfWork>();
			_funcionariosRepositoryMock.Setup(r => r.UnitOfWork).Returns(_unitOfWorkMock.Object);
			_handler = new UpdateFuncionarioCommandHandler(_funcionariosRepositoryMock.Object, _mediatorMock.Object);
		}

		[Fact]
		public async Task Handle_ShouldUpdateFuncionarioAndPublishEvent()
		{
			// Arrange
			var cancellationToken = new CancellationToken();
			var funcionario = new Funcionario("Usuario Jose", "jose_test@josemail.com.br", "Diretor de Finanças", DateTime.Parse("1995-11-24"), "josetest", "1aF2@+e4k", SituacaoFuncionario.Ativo);
			var command = new UpdateFuncionarioCommandInput(funcionario.Id, "Usuario Jose 2", "jose_test@josemail.com.br", "Diretor de Finanças", DateTime.Parse("1994-11-24"), "josetest2", "1aF2@+e4k", SituacaoFuncionario.Inativo);

			var funcionariosQueryable = new[] { funcionario }.AsQueryable();

			_funcionariosRepositoryMock
				   .Setup(r => r.GetAsync(It.IsAny<Expression<Func<Funcionario, bool>>>(), It.IsAny<CancellationToken>()))
				   .Returns((Expression<Func<Funcionario, bool>> predicate, CancellationToken ct) =>
					   Task.FromResult(funcionariosQueryable.FirstOrDefault(predicate)));

			// Act
			var result = await _handler.Handle(command, cancellationToken);

			// Assert
			_funcionariosRepositoryMock.Verify(x => x.Update(It.IsAny<Funcionario>()), Times.Once);
			_unitOfWorkMock.Verify(x => x.CommitAsync(cancellationToken), Times.Once);
			_mediatorMock.Verify(x => x.Publish(It.IsAny<UpdatedFuncionarioEventInput>(), cancellationToken), Times.Once);
			Assert.True(result);
		}

	}
}

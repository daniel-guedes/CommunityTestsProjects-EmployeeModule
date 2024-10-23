using Funcionarios.Application.Commands.AddFuncionarioCommand;
using Funcionarios.Application.Commands.DeleteFuncionarioCommand;
using Funcionarios.Application.Commands.UpdateFuncionarioCommand;
using Funcionarios.Application.Queries.GetFuncionarioByIdQuery;
using Funcionarios.Application.Queries.GetFuncionariosQuery;
using Funcionarios.Domain.EmployeeAggregate;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Funcionarios.Api.Controllers
{
    [ApiController]
	[Route("api/[controller]")]
	public class FuncionariosController(IMediator mediator) : ControllerBase
	{
		private readonly IMediator _mediator = mediator ?? throw new ArgumentException(nameof(mediator));

		[HttpGet]
		public async Task<ActionResult<List<FuncionarioModel>>> GetFuncionarios(CancellationToken cancellationToken)
			=> Ok(await _mediator.Send(new GetFuncionariosQueryInput(), cancellationToken));

		[HttpGet("{id:guid}")]
		public async Task<ActionResult<FuncionarioModel>> GetFuncionario([FromRoute] Guid id, CancellationToken cancellationToken)
		{
			var funcionario = await _mediator.Send(new GetFuncionarioByIdQueryInput(id), cancellationToken);

			if (funcionario == null)
				return NotFound();

			return Ok(funcionario);
		}

		[HttpPost]
		public async Task<ActionResult<int>> CreateFuncionario([FromBody] AddFuncionarioCommandInput command, CancellationToken cancellationToken)
		{
			var funcionarioId = await _mediator.Send(command, cancellationToken);

			if (funcionarioId == Guid.Empty)
				return BadRequest($"Funcionario não criado! Tente novamente ou entre em contato com desenvolvedor!");

			return CreatedAtAction(nameof(GetFuncionario), new { id = funcionarioId }, funcionarioId);
		}

		[HttpPut("{id:guid}")]
		public async Task<IActionResult> UpdateFuncionario([FromRoute] Guid id, [FromBody] AddFuncionarioCommandInput request, CancellationToken cancellationToken)
		{
			var command = new UpdateFuncionarioCommandInput(
				id, 
				request.NomeFuncionario, 
				request.Email, 
				request.Cargo, 
				request.DataNascimento, 
				request.Login, 
				request.Senha, 
				request.Situacao);
			var atualizado = await _mediator.Send(command, cancellationToken);

			if (!atualizado)
				return BadRequest($"Não foi possível atualizar o funcionario de id: {command.Id}.");

			return NoContent();
		}

		[HttpDelete("{id:guid}")]
		public async Task<IActionResult> DeleteFuncionario([FromRoute] Guid id, CancellationToken cancellationToken)
		{
			var removido = await _mediator.Send(new DeleteFuncionarioCommandInput(id), cancellationToken);

			if (!removido)
				return BadRequest($"Não foi possível remover o funcionario de id: {id}.");

			return NoContent();
		}
	}
}

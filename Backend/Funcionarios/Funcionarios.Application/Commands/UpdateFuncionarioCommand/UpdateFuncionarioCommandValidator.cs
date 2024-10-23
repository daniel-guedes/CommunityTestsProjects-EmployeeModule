using FluentValidation;
using Funcionarios.Domain.EmployeeAggregate;

namespace Funcionarios.Application.Commands.UpdateFuncionarioCommand;

public class UpdateFuncionarioCommandValidator : AbstractValidator<UpdateFuncionarioCommandInput>
{
    public UpdateFuncionarioCommandValidator()
	{
		RuleFor(x => x.NomeFuncionario)
			.MaximumLength(250)
			.WithMessage("O nome do funcionario pode possuir até 250 caracteres.");

		RuleFor(x => x.Email)
			.MaximumLength(250)
			.WithMessage("O email pode possuir até 250 caracteres.");

		RuleFor(x => x.Cargo)
			.MaximumLength(250)
			.WithMessage("O cargo pode possuir até 250 caracteres.");

		RuleFor(x => x.DataNascimento)
			.Must(date => date <= DateTime.Now)
			.WithMessage("A data de nascimento deve ser válida.");


		RuleFor(x => x.Login)
			.MinimumLength(4)
			.MaximumLength(40)
			.WithMessage("O login dever ter de 10 à 40 caracteres.");

		RuleFor(x => x.Senha)
			.MinimumLength(8)
			.Matches(@"[A-Z]+")
			.Matches(@"[a-z]+")
			.Matches(@"[0-9]+")
			.Matches(@"[^\w\d]+")
			.WithMessage("A senha deve conter pelo menos 8 caracteres, incluindo uma letra maiúscula, uma letra minúscula, um número e um caractere especial.");

		RuleFor(x => x.Situacao)
			.Must(situacao => Enum.IsDefined(typeof(SituacaoFuncionario), situacao))
			.WithMessage("O valor fornecido para Situacao está fora do intervalo permitido.");
	}
}

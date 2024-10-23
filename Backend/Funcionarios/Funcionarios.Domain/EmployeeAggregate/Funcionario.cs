using Funcionarios.Domain.DomainObjects;
using System.ComponentModel.DataAnnotations;

namespace Funcionarios.Domain.EmployeeAggregate;

public class Funcionario : Entity, IAggregateRoot
{
	protected Funcionario()
	{
	}

	public Funcionario(
		string nomeFuncionario,
		string email,
		string cargo,
		DateTime dataNascimento,
		string login,
		string senha,
		SituacaoFuncionario situacao
		)
	{
		WithNomeFuncionario(nomeFuncionario);
		WithEmail(email);
		WithCargo(cargo);
		WithDataNascimento(dataNascimento);
		WithLogin(login);
		WithSenha(senha);
		WithSituacao(situacao);
	}

	//ajuste fino: Não esquecer de criar regras de validação para os 
	public void WithNomeFuncionario(string nomeFuncionario) 
	{
		Validations.ValidarSeVazio(nomeFuncionario, "O campo NomeFuncionario não pode estar vazio.");
		Validations.ValidarSeNulo(nomeFuncionario, "O campo NomeFuncionario não pode ser nulo.");
		Validations.ValidarTamanho(nomeFuncionario, 250, "O campo NomeFuncionario não pode ser maior que 250 caracteres.");
	}
	public void WithEmail(string email)
	{
		Validations.ValidarSeVazio(email, "O campo Email não pode estar vazio.");
		Validations.ValidarSeNulo(email, "O campo Email não pode ser nulo.");
		Validations.ValidarFormatoEmail(email, "O formato do email é inválido.");

	}
	public void WithCargo(string cargo)
	{
		Validations.ValidarSeVazio(cargo, "O campo Cargo não pode estar vazio.");
		Validations.ValidarSeNulo(cargo, "O campo Cargo não pode ser nulo.");
		Validations.ValidarTamanho(cargo, 100, "O campo Cargo não pode ser maior que 100 caracteres.");

	}
	public void WithDataNascimento(DateTime dataNascimento)
	{
		if (dataNascimento > DateTime.Today)
		{
			throw new DomainException("A data de nascimento não pode ser futura.");
		}
	}
	public void WithLogin(string login)
	{
		Validations.ValidarSeVazio(login, "O campo Login não pode estar vazio.");
		Validations.ValidarSeNulo(login, "O campo Login não pode ser nulo.");
		Validations.ValidarTamanho(login, 50, "O campo Login não pode ser maior que 50 caracteres.");
	}
	public void WithSenha(string senha)
	{
		Validations.ValidarSeVazio(senha, "O campo Senha não pode estar vazio.");
		Validations.ValidarSeNulo(senha, "O campo Senha não pode ser nulo.");
	}
	public void WithSituacao(SituacaoFuncionario situacao)
	{
		if (!Enum.IsDefined(typeof(SituacaoFuncionario), situacao))
		{
			throw new ArgumentException("Situação do funcionário inválida.");
		}
	}

	[MaxLength(250)]
	public string NomeFuncionario { get; private set; } = string.Empty;
	public string Email { get; private set; } = string.Empty;
	public string Cargo { get; private set; } = string.Empty;
	public DateTime DataNascimento { get; private set; }
	public string Login { get; private set; } = string.Empty;
	public string Senha { get; private set; } = string.Empty;
	public SituacaoFuncionario Situacao { get; private set; }
}

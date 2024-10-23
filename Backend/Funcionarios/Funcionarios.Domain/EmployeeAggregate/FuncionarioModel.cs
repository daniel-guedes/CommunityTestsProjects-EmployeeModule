using Funcionarios.Domain.DomainObjects;

namespace Funcionarios.Domain.EmployeeAggregate;

public class FuncionarioModel : IMongoEntity
{
	public Guid Id { get; set; }
	public string NomeFuncionario { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string Cargo { get; set; } = string.Empty;
	public DateTime DataNascimento { get; set; }
	public string Login { get; set; } = string.Empty;
	public string Senha { get; set; } = string.Empty;
	public string Situacao { get; set; } = string.Empty;
}
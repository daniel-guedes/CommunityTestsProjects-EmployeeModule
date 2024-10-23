using Funcionarios.Domain.EmployeeAggregate;
using Funcionarios.Infra.Contexts;

namespace Funcionarios.Infra.Repositories.FuncionariosRepository;

public class FuncionariosRepository(SqlServerDbContext context) : GenericRepository<Funcionario>(context), IFuncionariosRepository
{
}

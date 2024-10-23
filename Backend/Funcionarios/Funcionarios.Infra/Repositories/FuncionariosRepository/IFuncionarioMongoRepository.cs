using Funcionarios.Domain.EmployeeAggregate;

namespace Funcionarios.Infra.Repositories.FuncionariosRepository;

public interface IFuncionarioMongoRepository : IMongoGenericRepository<FuncionarioModel>
{
}

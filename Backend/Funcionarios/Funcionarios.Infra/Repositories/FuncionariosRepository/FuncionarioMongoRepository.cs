using Funcionarios.Domain.EmployeeAggregate;
using MongoDB.Driver;

namespace Funcionarios.Infra.Repositories.FuncionariosRepository;

public class FuncionarioMongoRepository(IMongoDatabase database) : MongoGenericRepository<FuncionarioModel>(database, "funcionarios"), IFuncionarioMongoRepository
{
}

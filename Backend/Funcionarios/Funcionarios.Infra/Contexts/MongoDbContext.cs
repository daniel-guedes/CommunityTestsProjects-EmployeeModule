using Funcionarios.Domain.EmployeeAggregate;
using MongoDB.Driver;

namespace Funcionarios.Infra.Contexts;

public class MongoDbContext(IMongoDatabase database)
{
	private readonly IMongoDatabase _database = database;

	public IMongoCollection<FuncionarioModel> Funcionarios => _database.GetCollection<FuncionarioModel>("Funcionarios");
}


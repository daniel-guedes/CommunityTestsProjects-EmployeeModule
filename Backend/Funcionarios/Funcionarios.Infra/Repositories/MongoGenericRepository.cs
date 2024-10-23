using Funcionarios.Domain.DomainObjects;
using MongoDB.Driver;

namespace Funcionarios.Infra.Repositories;

public class MongoGenericRepository<T>(IMongoDatabase database, string collectionName) : IMongoGenericRepository<T> where T : IMongoEntity
{
	private readonly IMongoCollection<T> _collection = database.GetCollection<T>(collectionName);
	public async Task<bool> InsertAsync(T entity, CancellationToken cancellationToken)
	{
		await _collection.InsertOneAsync(entity, new InsertOneOptions(), cancellationToken);
		return true;
	}

	public Task<List<T>> GetAllAsync(CancellationToken cancellationToken)
		=> _collection.Find(_ => true).ToListAsync(cancellationToken);

	public Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken)
		=> _collection.Find(entity => entity.Id == id).FirstOrDefaultAsync(cancellationToken);


	public async Task<bool> RemoveAsync(Guid id, CancellationToken cancellationToken)
	{
		var result = await _collection.DeleteOneAsync(c => c.Id == id, cancellationToken);
		return result.IsAcknowledged && result.DeletedCount > 0;
	}

	public async Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken)
	{
		var result = await _collection.ReplaceOneAsync(c => c.Id == entity.Id, entity, new ReplaceOptions(), cancellationToken);
		return result.IsAcknowledged && result.ModifiedCount > 0;
	}
}

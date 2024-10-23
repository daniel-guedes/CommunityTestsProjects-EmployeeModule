using Funcionarios.Domain.DomainObjects;

namespace Funcionarios.Infra.Repositories;

public interface IMongoGenericRepository<T> where T : IMongoEntity
{
	Task<bool> InsertAsync(T entity, CancellationToken cancellationToken);
	Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken);
	Task<bool> RemoveAsync(Guid id, CancellationToken cancellationToken);
	Task<List<T>> GetAllAsync(CancellationToken cancellationToken);
	Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}

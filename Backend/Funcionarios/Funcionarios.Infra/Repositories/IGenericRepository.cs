using Funcionarios.Domain.Data;
using Funcionarios.Domain.DomainObjects;
using System.Linq.Expressions;

namespace Funcionarios.Infra.Repositories;

public interface IGenericRepository<TEntity> : IRepository where TEntity : IEntity
{
	Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default);
	void Add(TEntity entity);
	Task AddAsync(TEntity entity, CancellationToken cancellationToken);
	void Update(TEntity entity);
	void Remove(Guid id);
	void Remove(TEntity entity);
}

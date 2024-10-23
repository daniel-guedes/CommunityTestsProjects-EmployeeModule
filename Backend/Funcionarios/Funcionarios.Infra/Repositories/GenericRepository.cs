using Funcionarios.Domain.Data;
using Funcionarios.Domain.DomainObjects;
using Funcionarios.Infra.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace Funcionarios.Infra.Repositories;

public abstract class GenericRepository<TEntity>(SqlServerDbContext context) : IGenericRepository<TEntity> where TEntity : Entity
{
	private readonly SqlServerDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
	public IUnitOfWork UnitOfWork => _context;
	public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)
	{
		//verificar
		var query = _context
			.Set<TEntity>()
			.AsQueryable();

		if (predicate != null)
			query = query.Where(predicate);

		return await query.FirstOrDefaultAsync(cancellationToken);

	}

	public void Add(TEntity entity)
		=> _context.Add(entity);
	public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
		=> await _context.AddAsync(entity, cancellationToken);
	public void Update(TEntity entity)
		=> _context.Entry(entity).State = EntityState.Modified;
	public void Remove(Guid id)
		=> _context.Remove(id);
	public void Remove(TEntity entity)
		=> _context.Remove(entity);
}

using Funcionarios.Domain.Data;
using Funcionarios.Domain.DomainObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Funcionarios.Infra.Contexts;

public abstract class SqlServerContextBase(DbContextOptions<SqlServerDbContext> options) : DbContext(options), IUnitOfWork
{
	private static readonly ILoggerFactory LoggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(
		builder =>
		{
			builder
			.AddFilter((category, level) =>
				category == DbLoggerCategory.Database.Command.Name
			&& level == LogLevel.Information);

			builder.AddConsole();
		});

	public async Task<bool> CommitAsync(CancellationToken cancellationToken = default)
	{
		this.EnsureAutoHistory(() => new __EFAutoHistory());

		return await SaveChangesAsync(cancellationToken) > 0;
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
		=> optionsBuilder.UseLoggerFactory(LoggerFactory);

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.EnableAutoHistory<__EFAutoHistory>(o => { });

		base.OnModelCreating(modelBuilder);
	}
}

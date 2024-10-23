using Funcionarios.Domain.EmployeeAggregate;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Funcionarios.Infra.Contexts;

public class SqlServerDbContext(DbContextOptions<SqlServerDbContext> options) : SqlServerContextBase(options)
{
	public DbSet<Funcionario> Funcionarios { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		base.OnModelCreating(modelBuilder);
	}
}

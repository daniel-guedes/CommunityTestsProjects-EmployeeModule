namespace Funcionarios.Domain.Data;

public interface IUnitOfWork : IDisposable
{
	Task<bool> CommitAsync(CancellationToken cancellationToken = default);
}

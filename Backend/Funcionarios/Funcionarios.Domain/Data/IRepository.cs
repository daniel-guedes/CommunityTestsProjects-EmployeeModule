namespace Funcionarios.Domain.Data
{
	public interface IRepository
	{
		public IUnitOfWork UnitOfWork { get; }
	}
}

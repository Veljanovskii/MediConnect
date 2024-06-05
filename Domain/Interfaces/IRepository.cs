namespace Domain.Interfaces;

public interface IRepository<TEntity>
{
    Task<TEntity?> GetByIdAsync(int id);

    Task InsertAsync(TEntity entity);

    Task InsertAsync(IEnumerable<TEntity> entities);

    Task UpdateAsync(TEntity entity);

    Task UpdateAsync(IEnumerable<TEntity> entities);

    Task DeleteAsync(TEntity entity);

    Task DeleteAsync(IEnumerable<TEntity> entities);

    IQueryable<TEntity> Table { get; }

    Task<TEntity?> GetAsync(ISpecification<TEntity> spec);

    Task<IEnumerable<TEntity>> ListAsync(ISpecification<TEntity> spec);
}
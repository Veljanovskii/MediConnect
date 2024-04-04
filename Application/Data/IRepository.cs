using Microsoft.EntityFrameworkCore.Storage;

namespace Application.Data;

public interface IRepository<TEntity>
{
    /// <summary>
    /// Get entity by identifier
    /// </summary>
    /// <param name="id">Identifier</param>
    /// <returns>Entity</returns>
    Task<TEntity?> GetByIdAsync(int id);

    /// <summary>
    /// Insert entity
    /// </summary>
    /// <param name="entity">Entity</param>
    Task InsertAsync(TEntity entity);

    /// <summary>
    /// Insert entities
    /// </summary>
    /// <param name="entities">Entities</param>
    Task InsertAsync(IEnumerable<TEntity> entities);

    /// <summary>
    /// Update entity
    /// </summary>
    /// <param name="entity">Entity</param>
    Task UpdateAsync(TEntity entity);

    /// <summary>
    /// Update entities
    /// </summary>
    /// <param name="entities">Entities</param>
    Task UpdateAsync(IEnumerable<TEntity> entities);

    /// <summary>
    /// Delete entity
    /// </summary>
    /// <param name="entity">Entity</param>
    Task DeleteAsync(TEntity entity);

    /// <summary>
    /// Delete entities
    /// </summary>
    /// <param name="entities">Entities</param>
    Task DeleteAsync(IEnumerable<TEntity> entities);
    
    /// <summary>
    /// Begin Transaction
    /// </summary>
    virtual IDbContextTransaction BeginTransaction() { return default!; }

    /// <summary>
    /// Gets a table
    /// </summary>
    IQueryable<TEntity> Table { get; }
}
using Application.Data;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Base;

namespace Persistence;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<TEntity> _targetDbSet;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
        _targetDbSet = _context.Set<TEntity>();
    }

    /// <summary>
    /// Get entity by identifier
    /// </summary>
    /// <param name="id">Identifier</param>
    /// <returns>Entity</returns>
    public async Task<TEntity?> GetByIdAsync(int id)
    {
        return await _targetDbSet.FirstOrDefaultAsync(x => x.Id == id);
    }

    /// <summary>
    /// Insert entity
    /// </summary>
    /// <param name="entity">Entity</param>
    public async Task InsertAsync(TEntity entity)
    {
        await _targetDbSet.AddAsync(entity);

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Insert entities
    /// </summary>
    /// <param name="entities">Entities</param>
    public async Task InsertAsync(IEnumerable<TEntity> entities)
    {
        await _targetDbSet.AddRangeAsync(entities);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Update entity
    /// </summary>
    /// <param name="entity">Entity</param>
    public async Task UpdateAsync(TEntity entity)
    {
        _targetDbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Update entities
    /// </summary>
    /// <param name="entities">Entities</param>
    public async Task UpdateAsync(IEnumerable<TEntity> entities)
    {
        _targetDbSet.UpdateRange(entities);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Delete entity
    /// </summary>
    /// <param name="entity">Entity</param>
    public async Task DeleteAsync(TEntity entity)
    {
        _targetDbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Delete entities
    /// </summary>
    /// <param name="entities">Entities</param>
    public async Task DeleteAsync(IEnumerable<TEntity> entities)
    {
        _targetDbSet.RemoveRange(entities);
        await _context.SaveChangesAsync();
    }
    
    /// <summary>
    /// Begin Transaction
    /// </summary>
    public IDbContextTransaction BeginTransaction()
    {
        return _context!.Database.BeginTransaction();
    }

    /// <summary>
    /// Gets a table
    /// </summary>
    public IQueryable<TEntity> Table => _targetDbSet.AsQueryable();
}
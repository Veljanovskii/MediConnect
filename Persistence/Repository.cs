using Domain.Interfaces;
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

    public async Task<TEntity?> GetByIdAsync(int id)
    {
        return await _targetDbSet.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task InsertAsync(TEntity entity)
    {
        await _targetDbSet.AddAsync(entity);

        await _context.SaveChangesAsync();
    }

    public async Task InsertAsync(IEnumerable<TEntity> entities)
    {
        await _targetDbSet.AddRangeAsync(entities);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TEntity entity)
    {
        _targetDbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(IEnumerable<TEntity> entities)
    {
        _targetDbSet.UpdateRange(entities);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(TEntity entity)
    {
        _targetDbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(IEnumerable<TEntity> entities)
    {
        _targetDbSet.RemoveRange(entities);
        await _context.SaveChangesAsync();
    }
    
    public async Task<TEntity?> GetAsync(ISpecification<TEntity> spec)
    {
        var query = _targetDbSet.AsQueryable();

        // Apply criteria from specification
        if (spec.Criteria != null)
            query = query.Where(spec.Criteria);

        // Apply includes
        query = spec.Includes.Aggregate(query,
                (current, include) => current.Include(include));

        query = spec.IncludeStrings.Aggregate(query,
                (current, include) => current.Include(include));

        return await query.FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<TEntity>> ListAsync(ISpecification<TEntity> spec)
    {
        var query = _targetDbSet.AsQueryable();

        // Apply criteria from specification
        if (spec.Criteria != null)
            query = query.Where(spec.Criteria);

        // Apply includes
        foreach (var include in spec.Includes)
        {
            query = query.Include(include);
        }

        foreach (var includeString in spec.IncludeStrings)
        {
            query = query.Include(includeString);
        }

        return await query.ToListAsync();
    }

    public IQueryable<TEntity> Table => _targetDbSet.AsQueryable();
}
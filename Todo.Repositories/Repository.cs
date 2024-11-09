using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Todo.Models;

namespace Todo.Repositories;
public class Repository<T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext _db;
    internal DbSet<T> _set;

    public Repository(ApplicationDbContext db)
    {
        _db = db;
        _set = _db.Set<T>();
    }

    public async Task<T> Add(T entity)
    {
        var result = await _set.AddAsync(entity);
        await _db.SaveChangesAsync();
        return result.Entity;
    }

    public async Task Delete(T entity)
    {
        _set.Remove(entity);
        await _db.SaveChangesAsync();
    }

    public async Task<T?> Find(Expression<Func<T, bool>> filter)
    {
        IQueryable<T> query = _set;
        query = query.Where(filter);
        return await query.FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        IQueryable<T> query = _set;
        return await query.ToListAsync();
    }
}

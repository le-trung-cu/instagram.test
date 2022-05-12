using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Repository;
public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected RepositoryContext RepositoryContext;
    public RepositoryBase(RepositoryContext repositoryContext)
    => RepositoryContext = repositoryContext;

    public IQueryable<T> FindAll(bool trackChanges = false) =>
    !trackChanges ?
    RepositoryContext.Set<T>()
    .AsNoTracking() :
    RepositoryContext.Set<T>();

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression,bool trackChanges = false) =>
    !trackChanges ?
    RepositoryContext.Set<T>()
    .Where(expression)
    .AsNoTracking() :
    RepositoryContext.Set<T>()
    .Where(expression);

    public void Create(T entity) => RepositoryContext.Set<T>().Add(entity);
    public void Update(T entity) => RepositoryContext.Set<T>().Update(entity);
    public void Delete(T entity)
    {
        if (typeof(SoftDelete).IsAssignableFrom(typeof(T)))
        {
            (entity as SoftDelete)!.DeletedAt = DateTimeOffset.UtcNow;
            RepositoryContext.Attach(entity);
            RepositoryContext.Entry(entity).State = EntityState.Modified;
        }
        else
        {
            RepositoryContext.Set<T>().Remove(entity);
        }
    } 
}


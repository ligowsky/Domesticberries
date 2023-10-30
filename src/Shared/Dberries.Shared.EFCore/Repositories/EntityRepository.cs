using System.Reflection;
using BitzArt;
using Microsoft.EntityFrameworkCore;

namespace Dberries;

public abstract class EntityRepository : RepositoryBase, IEntityRepository
{
    protected EntityRepository(AppDbContext db) : base(db)
    {
    }
    
    public async Task<bool> CheckExistsAsync<T>(Guid id, bool throwException = false) where T : class, IEntity
    {
        var entityExists = await Db.Set<T>()
            .Where(x => x.Id == id)
            .AnyAsync();

        if (!entityExists && throwException)
            throw ApiException.NotFound($"{typeof(T).Name} with Id '{id}' is not found");

        return entityExists;
    }

    public async Task<bool> CheckExistsByExternalIdAsync<TExternalKey>(Type entityType, TExternalKey id,
        bool throwException = false)
    {
        var typeImplementsInterface = entityType.GetInterfaces()
            .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEntityWithExternalKey<>));

        if (!typeImplementsInterface)
            throw new ArgumentException(
                $"{entityType.Name} must implement {typeof(IEntityWithExternalKey<>).Name} interface");

        return await (Task<bool>)GetType()
            .GetMethods(BindingFlags.NonPublic)
            .First(x => x.Name == nameof(CheckExistsByExternalIdAsync))
            .MakeGenericMethod(entityType, typeof(TExternalKey))
            .Invoke(this, new object[] { id!, throwException })!;
    }

    private async Task<bool> CheckExistsByExternalIdAsync<TEntity, TExternalKey>(TExternalKey id,
        bool throwException = false) where TEntity : class, IEntityWithExternalKey<TExternalKey>
    {
        var entityExists = await Db.Set<TEntity>()
            .Where(x => x.ExternalId!.Equals(id))
            .AnyAsync();

        if (entityExists && throwException)
            throw ApiException.BadRequest($"{typeof(TEntity).Name} with ExternalId '{id}' already exists");

        return entityExists;
    }
}
using System.Reflection;
using BitzArt;
using Microsoft.EntityFrameworkCore;

namespace Dberries;

public static class DbContextExtensions
{
    public static async Task<bool> CheckExistsAsync<T>(this DbContext db, Guid id, bool throwException = false)
        where T : class, IEntity
    {
        var entityExists = await db.Set<T>()
            .Where(x => x.Id == id)
            .AnyAsync();

        if (!entityExists && throwException)
            throw ApiException.NotFound($"{typeof(T).Name} with Id '{id}' is not found");

        return entityExists;
    }
    
    public static async Task<bool> CheckExistsByExternalIdAsync<TExternalKey>(this DbContext db, Type entityType, TExternalKey id,
        bool throwException = false)
    {
        var typeImplementsInterface = entityType.GetInterfaces()
            .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEntityWithExternalKey<>));

        if (!typeImplementsInterface)
            throw new ArgumentException(
                $"{entityType.Name} must implement {typeof(IEntityWithExternalKey<>).Name} interface");

        return await (Task<bool>)typeof(DbContextExtensions)
            .GetMethods(BindingFlags.NonPublic)
            .First(x => x.Name == nameof(CheckExistsByExternalIdAsync))
            .MakeGenericMethod(entityType, typeof(TExternalKey))
            .Invoke(null, new object[] { db, id!, throwException })!;
    }

    private static async Task<bool> CheckExistsByExternalIdAsync<TEntity, TExternalKey>(DbContext db, TExternalKey id,
        bool throwException = false) where TEntity : class, IEntityWithExternalKey<TExternalKey>
    {
        var entityExists = await db.Set<TEntity>()
            .Where(x => x.ExternalId!.Equals(id))
            .AnyAsync();

        if (entityExists && throwException)
            throw ApiException.BadRequest($"{typeof(TEntity).Name} with ExternalId '{id}' already exists");

        return entityExists;
    }
}
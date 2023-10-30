using System.Reflection;
using BitzArt;
using Microsoft.EntityFrameworkCore;

namespace Dberries;

public static class DbContextExtensions
{
    public static async Task<bool> CheckIfExistsAsync<T>(this DbContext db, Guid id)
        where T : class, IEntity
    {
        return await db.Set<T>()
            .Where(x => x.Id == id)
            .AnyAsync();
    }

    public static async Task ThrowIfExistsAsync<T>(this DbContext db, Guid id)
        where T : class, IEntity
    {
        var entityExists = await CheckIfExistsAsync<T>(db, id);

        if (entityExists)
            throw ApiException.NotFound($"{typeof(T).Name} with Id '{id}' already exists");
    }

    public static async Task ThrowIfNotExistsAsync<T>(this DbContext db, Guid id)
        where T : class, IEntity
    {
        var entityExists = await CheckIfExistsAsync<T>(db, id);

        if (!entityExists)
            throw ApiException.NotFound($"{typeof(T).Name} with Id '{id}' is not found");
    }

    public static async Task<bool> CheckIfExistsByExternalIdAsync<TExternalKey>(this DbContext db, Type entityType,
        TExternalKey id)
    {
        var typeImplementsInterface = entityType.GetInterfaces()
            .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEntityWithExternalId<>));

        if (!typeImplementsInterface)
            throw new ArgumentException(
                $"{entityType.Name} must implement {typeof(IEntityWithExternalId<>).Name} interface");

        return await (Task<bool>)typeof(DbContextExtensions)
            .GetMethods(BindingFlags.NonPublic)
            .First(x => x.Name == nameof(CheckIfExistsByExternalIdAsync))
            .MakeGenericMethod(entityType, typeof(TExternalKey))
            .Invoke(null, new object[] { db, id! })!;
    }

    private static async Task<bool> CheckIfExistsByExternalIdAsync<TEntity, TExternalKey>(DbContext db, TExternalKey id)
        where TEntity : class, IEntityWithExternalId<TExternalKey>
        where TExternalKey : struct
    {
        return await db.Set<TEntity>()
            .Where(x => x.ExternalId!.Equals(id))
            .AnyAsync();
    }

    public static async Task ThrowIfExistsByExternalIdAsync<TExternalKey>(this DbContext db, Type entityType,
        TExternalKey id)
    {
        var entityExists = await CheckIfExistsByExternalIdAsync(db, entityType, id);

        if (entityExists)
            throw ApiException.BadRequest($"{entityType.Name} with ExternalId '{id}' already exists");
    }

    public static async Task ThrowIfNotExistsByExternalIdAsync<TExternalKey>(this DbContext db, Type entityType,
        TExternalKey id)
    {
        var entityExists = await CheckIfExistsByExternalIdAsync(db, entityType, id);

        if (!entityExists)
            throw ApiException.BadRequest($"{entityType.Name} with ExternalId '{id}' is not found");
    }
}
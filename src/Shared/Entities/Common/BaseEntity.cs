namespace Shared;

public class BaseEntity
{
    public Guid? Id { get; set; }

    protected BaseEntity(Guid? id)
    {
        Id = id;
    }
}
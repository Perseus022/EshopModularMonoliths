namespace Shared.DDD;

public interface IEntity<T>:IEntity
{
    public T Id { get; set; }
 }

public interface IEntity
{
    public DateTime CreatedAt { get; set; }
    public String? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public String? LastModifiedBy { get; set; }
 
}

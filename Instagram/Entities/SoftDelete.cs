namespace Entities
{
    public interface SoftDelete
    {
        public DateTimeOffset DeletedAt { get; set; }
    }
}

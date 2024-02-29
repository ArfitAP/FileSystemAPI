
namespace FileSystemAPI.Domain.Common
{
    /// <summary>
    /// Common entity properties
    /// </summary>
    public class AuditableEntity
    {
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}

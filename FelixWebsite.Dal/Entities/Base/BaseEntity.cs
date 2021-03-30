using System;

namespace FelixWebsite.Dal.Entities.Base
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public bool AuditDeleted { get; set; }
        public int? AuditDeletedBy { get; set; }
        public DateTime? AuditDeletedOn { get; set; }

        public void Delete(int userId)
        {
            AuditDeleted = true;
            AuditDeletedBy = userId;
            AuditDeletedOn = DateTime.Now;
        }
    }
}

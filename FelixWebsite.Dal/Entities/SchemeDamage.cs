using FelixWebsite.Dal.Entities.Base;
using FelixWebsite.Shared.enums;

namespace FelixWebsite.Dal.Entities
{
    public class SchemeDamage:BaseEntity
    {
        public int UserId { get; set; }

        public SchemeEntry SchemeEntry { get; set; }

        public int OrderNumber => (int)SchemeEntry + 1;

        public bool IsSelected { get; set; }

        public bool Damage { get; set; }

        public bool HeavyDamage  { get; set; }

        public bool Dent { get; set; }

        public bool Marks { get; set; }

        public bool HailDamage { get; set; }

        public string Remarks { get; set; }

    }
}

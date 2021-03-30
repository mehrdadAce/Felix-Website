using FelixWebsite.Bdo.Models.Base;
using FelixWebsite.Shared.enums;
using System.Collections.Generic;

namespace FelixWebsite.Bdo.Models.Acquisition
{
    public class SchemeDamage:BaseModel
    {
        public int UserId { get; set; }

        public SchemeEntry SchemeEntry { get; set; }

        public int OrderNumber { get; set; }

        public bool IsSelected { get; set; }

        public bool Damage { get; set; }

        public bool HeavyDamage { get; set; }

        public bool Marks { get; set; }

        public bool HailDamage { get; set; }

        public bool Dent { get; set; }

        public string Remarks { get; set; }

        public List<MediaValuesModel> Pictures { get; set; }

    }
}

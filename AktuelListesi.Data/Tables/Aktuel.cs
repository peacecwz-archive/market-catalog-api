using System;
using System.Collections.Generic;
using System.Text;

namespace AktuelListesi.Data.Tables
{
    public class Aktuel : BaseEntity<int>
    {
        public string ReleasedDate { get; set; }
        public int ComapnyId { get; set; }

        public virtual Company Company { get; set; }
        public virtual IEnumerable<AktuelPage> AktuelPages { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace AktuelListesi.Data.Tables
{
    public class AktuelPage : BaseEntity<int>
    {
        public string Content { get; set; }
        public string PageImageUrl { get; set; }
        public string OriginalImageUrl { get; set; }
        public int AktuelId { get; set; }

        public virtual Aktuel Aktuel { get; set; }

    }
}

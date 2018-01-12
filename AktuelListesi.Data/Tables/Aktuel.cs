using System;
using System.Collections.Generic;
using System.Text;

namespace AktuelListesi.Data.Tables
{
    public class Aktuel : BaseEntity<int>
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string OriginalImageUrl { get; set; }
        public bool IsLatest { get; set; } = true;
        /// <summary>
        /// It's for Crawler
        /// </summary>
        public int NewsId { get; set; }
        public DateTime ReleasedDate { get; set; }
        public string ReleasedDateString { get; set; }
        public int CompanyId { get; set; }

        public virtual Company Company { get; set; }
        public virtual IEnumerable<AktuelPage> AktuelPages { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace AktuelListesi.Data.Tables
{
    public class Company :BaseEntity<int>
    {
        /// <summary>
        /// It's for Crawler
        /// </summary>
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }

        public virtual IEnumerable<Aktuel> Aktuels { get; set; }
    }
}

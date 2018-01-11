using System;
using System.Collections.Generic;
using System.Text;

namespace AktuelListesi.Data.Dtos
{
    public class AktuelDto : BaseDto<int>
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string OriginalImageUrl { get; set; }
        public bool IsLatest { get; set; }
        /// <summary>
        /// It's for Crawler
        /// </summary>
        public int NewsId { get; set; }
        public string ReleasedDate { get; set; }
        public int CompanyId { get; set; }
    }
}

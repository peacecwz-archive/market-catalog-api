using System;
using System.Collections.Generic;
using System.Text;

namespace AktuelListesi.Data.Dtos
{
    public class CompanyDto : BaseDto<int>
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        
    }
}

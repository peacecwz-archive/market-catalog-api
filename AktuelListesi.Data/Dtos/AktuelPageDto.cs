using System;
using System.Collections.Generic;
using System.Text;

namespace AktuelListesi.Data.Dtos
{
    public class AktuelPageDto : BaseDto<int>
    {
        public string Content { get; set; }
        public string PageImageUrl { get; set; }
        public int AktuelId { get; set; }
        

    }
}

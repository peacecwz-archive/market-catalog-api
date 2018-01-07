using System;
using System.Collections.Generic;
using System.Text;

namespace AktuelListesi.Data.Dtos
{
    public class AktuelDto : BaseDto<int>
    {
        public string ReleasedDate { get; set; }
        public int ComapnyId { get; set; }
    }
}

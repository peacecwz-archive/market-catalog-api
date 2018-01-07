using System;
using System.Collections.Generic;
using System.Text;

namespace AktuelListesi.Data
{
    public class BaseDto<T>
    {
        public T Id { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

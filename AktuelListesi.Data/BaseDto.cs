using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AktuelListesi.Data
{
    public class BaseDto<T>
    {
        [Key]
        public T Id { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; } 
    }
}

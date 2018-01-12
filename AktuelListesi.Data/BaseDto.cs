using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AktuelListesi.Data
{
    public class BaseDto<T>
    {
        [Key]
        public T Id { get; set; } = default(T);
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace AktuelListesi.Models.Helpers
{
    public class ActionResultModel
    {
        public List<string> Messages { get; set; }
        public bool IsSuccess { get; set; }
        public object Data { get; set; }
    }
}

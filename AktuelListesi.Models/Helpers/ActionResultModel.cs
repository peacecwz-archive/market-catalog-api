using System;
using System.Collections.Generic;
using System.Text;

namespace AktuelListesi.Models.Helpers
{
    public class ActionResultModel
    {
        public ActionResultModel()
        {
            Messages = new List<string>();
        }

        public List<string> Messages { get; set; }
        public bool IsSuccess { get; set; }
        public object Data { get; set; }
    }
}

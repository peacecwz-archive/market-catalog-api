using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AktuelListesi.Models.AppServices
{
    public enum QueueType
    {
        [Description("aktuellistesi-update")]
        Update = 1,
        [Description("aktuellistesi-content")]
        ContentUpdate = 2
    }
}

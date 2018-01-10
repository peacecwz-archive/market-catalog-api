using AktuelListesi.Data.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace AktuelListesi.Models.App
{
    public class LatestModel
    {
        public IEnumerable<CompanyDto> Companies { get; set; }
        public IEnumerable<AktuelDto> Aktuels { get; set; }
    }
}

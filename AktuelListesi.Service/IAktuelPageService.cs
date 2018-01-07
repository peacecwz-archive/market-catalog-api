using AktuelListesi.Data.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace AktuelListesi.Service
{
    public interface IAktuelPageService
    {
        IEnumerable<AktuelPageDto> GetAktuelPages();
        AktuelPageDto GetAktuelPage(int Id);
        AktuelPageDto AddAktuelPage(AktuelPageDto dto);
        AktuelPageDto UpdateAktuelPage(AktuelPageDto dto);
        bool SoftDeleteAktuelPage(int Id);
        bool HardDeleteAktuelPage(int Id);

    }
}

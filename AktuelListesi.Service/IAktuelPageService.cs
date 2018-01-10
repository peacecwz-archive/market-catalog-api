using AktuelListesi.Data.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace AktuelListesi.Service
{
    public interface IAktuelPageService
    {
        IEnumerable<AktuelPageDto> GetAktuelPages();
        IEnumerable<AktuelPageDto> GetAktuelPagesByAktuelId(int AktuelId);
        AktuelPageDto GetAktuelPage(int Id);
        AktuelPageDto AddAktuelPage(AktuelPageDto dto);
        AktuelPageDto AddOrGetAktuelPage(AktuelPageDto dto);
        AktuelPageDto UpdateAktuelPage(AktuelPageDto dto);
        bool SoftDeleteAktuelPage(int Id);
        bool HardDeleteAktuelPage(int Id);

    }
}

using AktuelListesi.Data.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace AktuelListesi.DataService
{
    public interface ICompanyService
    {
        IEnumerable<CompanyDto> GetCompanies();
        CompanyDto GetCompany(int Id);
        CompanyDto AddCompany(CompanyDto dto);
        CompanyDto AddOrGetCompany(CompanyDto dto);
        CompanyDto UpdateCompany(CompanyDto dto);
        bool SoftDeleteCompany(int Id);
        bool HardDeleteCompany(int Id);
    }
}

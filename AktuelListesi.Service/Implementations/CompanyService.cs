using System;
using System.Collections.Generic;
using System.Text;
using AktuelListesi.Data.Dtos;
using AktuelListesi.Data.Tables;
using AktuelListesi.Repository;

namespace AktuelListesi.Service.Implementations
{
    public class CompanyService : ICompanyService
    {
        private readonly IRepository<Company, CompanyDto> repository;
        public CompanyService(IRepository<Company, CompanyDto> repository)
        {
            this.repository = repository;
        }

        public CompanyDto AddCompany(CompanyDto dto)
        {
            if (repository.Add(dto))
                return dto;
            return null;
        }

        public IEnumerable<CompanyDto> GetCompanies()
        {
            return repository.All();
        }

        public CompanyDto GetCompany(int Id)
        {
            return repository.GetById<int>(Id);
        }

        public bool HardDeleteCompany(int Id)
        {
            return repository.Delete<int>(GetCompany(Id), isSoftDelete: false);
        }

        public bool SoftDeleteCompany(int Id)
        {
            return repository.Delete<int>(GetCompany(Id), isSoftDelete: true);
        }

        public CompanyDto UpdateCompany(CompanyDto dto)
        {
            if (repository.Update(dto))
                return dto;
            return null;
        }
    }
}

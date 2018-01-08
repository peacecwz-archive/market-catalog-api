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
            if (repository.Add(dto) != null)
                return dto;
            return null;
        }

        public CompanyDto AddOrGetCompany(CompanyDto dto)
        {
            var company = repository.First(x => x.Name == dto.Name | x.CategoryId == dto.CategoryId);
            if (company == null) return (((dto = repository.Add(dto)) != null) ? dto : null);

            return company;
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
            return repository.Delete<int>(GetCompany(Id), isSoftDelete: false) != null;
        }

        public bool SoftDeleteCompany(int Id)
        {
            return repository.Delete<int>(GetCompany(Id), isSoftDelete: true) != null;
        }

        public CompanyDto UpdateCompany(CompanyDto dto)
        {
            if (repository.Update(dto) != null)
                return dto;
            return null;
        }
    }
}

using AktuelListesi.Data.Dtos;
using AktuelListesi.Data.Tables;
using AktuelListesi.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace AktuelListesi.DataService.Implementations
{
    public class AktuelService : IAktuelService
    {
        private readonly IRepository<Aktuel, AktuelDto> repository;
        public AktuelService(IRepository<Aktuel, AktuelDto> repository)
        {
            this.repository = repository;
        }

        public AktuelDto AddAktuel(AktuelDto dto)
        {
            if (repository.Add(dto) != null)
                return dto;
            return null;
        }

        public IEnumerable<AktuelDto> GetAktuels()
        {
            return repository.All();
        }

        public AktuelDto GetAktuel(int Id)
        {
            return repository.GetById<int>(Id);
        }

        public bool HardDeleteAktuel(int Id)
        {
            return repository.Delete<int>(GetAktuel(Id), isSoftDelete: false) != null;
        }

        public bool SoftDeleteAktuel(int Id)
        {
            return repository.Delete<int>(GetAktuel(Id), isSoftDelete: true) != null;
        }

        public AktuelDto UpdateAktuel(AktuelDto dto)
        {
            if (repository.Update(dto) != null)
                return dto;
            return null;
        }

        public AktuelDto AddOrGetAktuel(AktuelDto dto)
        {
            var aktuel = repository.First(x => x.NewsId == dto.NewsId & x.CompanyId == dto.CompanyId);
            if (aktuel == null) return ((dto = this.AddAktuel(dto)) != null) ? dto : null;

            return aktuel;
        }

        public IEnumerable<AktuelDto> GetLatestAktuels()
        {
            return repository.WhereActives<int>(x => x.IsLatest).OrderByDescending(x => x.NewsId);
        }

        public IEnumerable<AktuelDto> GetAktuelsByCompanyId(int CompanyId)
        {
            return repository.Where(x => x.CompanyId == CompanyId);
        }

        public bool DeactiveLatestAktuels()
        {
            try
            {
                var latestAktuels = GetLatestAktuels()?.ToList();
                latestAktuels?.ForEach(aktuel => aktuel.IsLatest = false);
                
                repository.UpdateRange(latestAktuels);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<AktuelDto> Search(string query)
        {
            return this.repository.Table.Include(x => x.AktuelPages)
                                        .Where(x => x.AktuelPages.Any(y => y.Content.Contains(query)))
                                        .Select(x => repository.Mapper.Map<Aktuel, AktuelDto>(x));
        }
    }
}

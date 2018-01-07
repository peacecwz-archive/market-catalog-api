using AktuelListesi.Data.Dtos;
using AktuelListesi.Data.Tables;
using AktuelListesi.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace AktuelListesi.Service.Implementations
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
            if (repository.Add(dto))
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
            return repository.Delete<int>(GetAktuel(Id), isSoftDelete: false);
        }

        public bool SoftDeleteAktuel(int Id)
        {
            return repository.Delete<int>(GetAktuel(Id), isSoftDelete: true);
        }

        public AktuelDto UpdateAktuel(AktuelDto dto)
        {
            if (repository.Update(dto))
                return dto;
            return null;
        }
    }
}

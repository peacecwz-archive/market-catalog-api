using AktuelListesi.Data.Dtos;
using AktuelListesi.Data.Tables;
using AktuelListesi.Repository;
using AktuelListesi.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace AktuelListesi.Service.Implementations
{
    public class AktuelPageService : IAktuelPageService
    {
        private readonly IRepository<AktuelPage, AktuelPageDto> repository;
        public AktuelPageService(IRepository<AktuelPage, AktuelPageDto> repository)
        {
            this.repository = repository;
        }

        public AktuelPageDto AddAktuelPage(AktuelPageDto dto)
        {
            if (repository.Add(dto))
                return dto;
            return null;
        }

        public IEnumerable<AktuelPageDto> GetAktuelPages()
        {
            return repository.All();
        }

        public AktuelPageDto GetAktuelPage(int Id)
        {
            return repository.GetById<int>(Id);
        }

        public bool HardDeleteAktuelPage(int Id)
        {
            return repository.Delete<int>(GetAktuelPage(Id), isSoftDelete: false);
        }

        public bool SoftDeleteAktuelPage(int Id)
        {
            return repository.Delete<int>(GetAktuelPage(Id), isSoftDelete: true);
        }

        public AktuelPageDto UpdateAktuelPage(AktuelPageDto dto)
        {
            if (repository.Update(dto))
                return dto;
            return null;
        }
    }
}

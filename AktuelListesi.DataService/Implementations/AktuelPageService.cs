using AktuelListesi.Data.Dtos;
using AktuelListesi.Data.Tables;
using AktuelListesi.Repository;
using AktuelListesi.DataService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AktuelListesi.DataService.Implementations
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
            if (repository.Add(dto) != null)
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
            return repository.Delete<int>(GetAktuelPage(Id), isSoftDelete: false) != null;
        }

        public bool SoftDeleteAktuelPage(int Id)
        {
            return repository.Delete<int>(GetAktuelPage(Id), isSoftDelete: true) != null;
        }

        public AktuelPageDto UpdateAktuelPage(AktuelPageDto dto)
        {
            var orgDto = repository.Single(x => x.AktuelId == dto.AktuelId && x.PageImageUrl == dto.PageImageUrl);
            if (orgDto != null)
            {
                dto.Id = orgDto.Id;
                if (repository.Update(dto) != null)
                    return dto;
            }
            return null;
        }

        public AktuelPageDto AddOrGetAktuelPage(AktuelPageDto dto)
        {
            var aktuelPage = repository.First(x => x.AktuelId == dto.AktuelId & x.OriginalImageUrl == dto.OriginalImageUrl);
            if (aktuelPage == null) return ((dto = repository.Add(dto)) != null) ? dto : null;

            return aktuelPage;
        }

        public void AddRange(List<AktuelPageDto> dtos)
        {
            dtos = dtos.Where(x => !repository.Table.Any(y => y.AktuelId == x.AktuelId && y.OriginalImageUrl == x.OriginalImageUrl)).ToList();
            repository.AddRange(dtos);
        }

        public IEnumerable<AktuelPageDto> GetAktuelPagesByAktuelId(int AktuelId)
        {
            return repository.Where(x => x.AktuelId == AktuelId);
        }
    }
}

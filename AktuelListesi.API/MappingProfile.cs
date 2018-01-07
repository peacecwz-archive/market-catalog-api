using AktuelListesi.Data.Dtos;
using AktuelListesi.Data.Tables;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AktuelListesi.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Company, CompanyDto>();
            CreateMap<CompanyDto, Company>();
            CreateMap<Aktuel, AktuelDto>();
            CreateMap<AktuelDto, Aktuel>();
            CreateMap<AktuelPage, AktuelPageDto>();
            CreateMap<AktuelPageDto, AktuelPage>();
        }
    }
}

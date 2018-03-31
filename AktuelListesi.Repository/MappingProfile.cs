using AktuelListesi.Data.Dtos;
using AktuelListesi.Data.Tables;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace AktuelListesi.Repository
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

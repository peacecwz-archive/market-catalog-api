using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AktuelListesi.Data.Dtos;
using AktuelListesi.Data.Tables;
using AktuelListesi.Repository;
using Microsoft.AspNetCore.Mvc;

namespace AktuelListesi.Admin.Controllers
{
    public class AktuelsController : CRUDController<Aktuel, AktuelDto, int>
    {
        public AktuelsController(IRepository<Aktuel, AktuelDto> repository)
            : base(repository)
        {
        }
    }
}
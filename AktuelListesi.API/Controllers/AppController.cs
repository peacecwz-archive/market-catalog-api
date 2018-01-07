using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AktuelListesi.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AktuelListesi.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AppController : ApiController
    {
        private readonly IAktuelPageService aktuelPageService;
        private readonly IAktuelService aktuelService;
        private readonly ICompanyService companyService;
        public AppController(IAktuelPageService aktuelPageService,
                             IAktuelService aktuelService,
                             ICompanyService companyService){
            this.aktuelPageService = aktuelPageService;
            this.aktuelService = aktuelService;
            this.companyService = companyService;
        }

        [HttpGet]
        public IActionResult Update(){
            return Ok();
        }
    }
}
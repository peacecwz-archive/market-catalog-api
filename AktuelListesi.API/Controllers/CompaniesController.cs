using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AktuelListesi.Data.Dtos;
using AktuelListesi.Service;
using Microsoft.AspNetCore.Mvc;

namespace AktuelListesi.API.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    public class CompaniesController : ApiController
    {
        private readonly ICompanyService companyService;
        public CompaniesController(ICompanyService companyService)
        {
            this.companyService = companyService;
        }

        // GET api/companies
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(companyService.GetCompanies());
        }

        // GET api/companies/5
        [HttpGet("{id}")]
        public IActionResult Get(int? id)
        {
            if (!id.HasValue) return BadRequest();

            var company = companyService.GetCompany(id.Value);
            if (company == null) return NotFound();

            return Ok(company);
        }

        // POST api/companies
        [HttpPost]
        public IActionResult Post([FromBody]CompanyDto company)
        {
            if (ModelState.IsValid && companyService.AddCompany(company) != null)
            {
                return Ok(company);
            }

            return BadRequest();
        }

        // PUT api/companies/5
        [HttpPut("{id}")]
        public IActionResult Put(int? id, [FromBody]CompanyDto company)
        {
            if (!id.HasValue) return BadRequest();

            var comp = companyService.GetCompany(id.Value);
            if (comp == null) return NotFound();

            company.Id = comp.Id;
            if (ModelState.IsValid && companyService.UpdateCompany(company) != null)
            {
                return Ok(company);
            }

            return BadRequest();
        }

        // DELETE api/companies/5
        [HttpDelete("{id}/{isSoftDelete}")]
        public IActionResult Delete(int? id, bool isSoftDelete = true)
        {
            if (!id.HasValue) return BadRequest();

            var company = companyService.GetCompany(id.Value);
            if (company == null) return NotFound();

            bool isSuccess = (isSoftDelete) ?
                                companyService.SoftDeleteCompany(id.Value) :
                                companyService.HardDeleteCompany(id.Value);

            return Ok(isSuccess);
        }
    }
}

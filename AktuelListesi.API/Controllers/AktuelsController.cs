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
    public class AktuelsController : ApiController
    {
        private readonly IAktuelService aktuelService;
        public AktuelsController(IAktuelService aktuelService)
        {
            this.aktuelService = aktuelService;
        }

        // GET api/aktuels
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(aktuelService.GetAktuels());
        }

        // GET api/aktuels/5
        [HttpGet("{id}")]
        public IActionResult Get(int? id)
        {
            if (!id.HasValue) return BadRequest();

            var company = aktuelService.GetAktuel(id.Value);
            if (company == null) return NotFound();

            return Ok(company);
        }

        // POST api/aktuels
        [HttpPost]
        public IActionResult Post([FromBody]AktuelDto company)
        {
            if (ModelState.IsValid && aktuelService.AddAktuel(company) != null)
            {
                return Ok(company);
            }

            return BadRequest();
        }

        // PUT api/aktuels/5
        [HttpPut("{id}")]
        public IActionResult Put(int? id, [FromBody]AktuelDto company)
        {
            if (!id.HasValue) return BadRequest();

            var comp = aktuelService.GetAktuel(id.Value);
            if (comp == null) return NotFound();

            if (ModelState.IsValid && aktuelService.UpdateAktuel(id.Value, company) != null)
            {
                return Ok(company);
            }

            return BadRequest();
        }

        // DELETE api/aktuels/5
        [HttpDelete("{id}/{isSoftDelete}")]
        public IActionResult Delete(int? id, bool isSoftDelete = true)
        {
            if (!id.HasValue) return BadRequest();

            var company = aktuelService.GetAktuel(id.Value);
            if (company == null) return NotFound();

            bool isSuccess = (isSoftDelete) ?
                                aktuelService.SoftDeleteAktuel(id.Value) :
                                aktuelService.HardDeleteAktuel(id.Value);

            return Ok(isSuccess);
        }
    }
}

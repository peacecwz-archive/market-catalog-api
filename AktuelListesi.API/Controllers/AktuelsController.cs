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

            var aktuel = aktuelService.GetAktuel(id.Value);
            if (aktuel == null) return NotFound();

            return Ok(aktuel);
        }

        // POST api/aktuels
        [HttpPost]
        public IActionResult Post([FromBody]AktuelDto aktuel)
        {
            if (ModelState.IsValid && aktuelService.AddAktuel(aktuel) != null)
            {
                return Ok(aktuel);
            }

            return BadRequest();
        }

        // PUT api/aktuels/5
        [HttpPut("{id}")]
        public IActionResult Put(int? id, [FromBody]AktuelDto aktuel)
        {
            if (!id.HasValue) return BadRequest();

            var akt = aktuelService.GetAktuel(id.Value);
            if (akt == null) return NotFound();
            aktuel.Id = akt.Id;
            if (ModelState.IsValid && aktuelService.UpdateAktuel(aktuel) != null)
            {
                return Ok(aktuel);
            }

            return BadRequest();
        }

        // DELETE api/aktuels/5
        [HttpDelete("{id}/{isSoftDelete}")]
        public IActionResult Delete(int? id, bool isSoftDelete = true)
        {
            if (!id.HasValue) return BadRequest();

            var aktuel = aktuelService.GetAktuel(id.Value);
            if (aktuel == null) return NotFound();

            bool isSuccess = (isSoftDelete) ?
                                aktuelService.SoftDeleteAktuel(id.Value) :
                                aktuelService.HardDeleteAktuel(id.Value);

            return Ok(isSuccess);
        }
    }
}

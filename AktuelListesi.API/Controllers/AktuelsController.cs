using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AktuelListesi.Data.Dtos;
using AktuelListesi.DataService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AktuelListesi.API.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    public class AktuelsController : ApiController
    {
        private readonly IAktuelService aktuelService;
        private readonly IAktuelPageService aktuelPageService;
        public AktuelsController(IAktuelService aktuelService,
                                 IAktuelPageService aktuelPageService)
        {
            this.aktuelService = aktuelService;
            this.aktuelPageService = aktuelPageService;
        }

        // GET api/v1/aktuels
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(aktuelService.GetAktuels());
        }

        // GET api/v1/aktuels/5/aktuelPages
        [HttpGet("{id}/aktuelPages")]
        public IActionResult GetAktuelPages(int? id)
        {
            if (!id.HasValue) return BadRequest();

            var aktuel = aktuelService.GetAktuel(id.Value);
            if (aktuel == null) return NotFound();

            var aktuelPages = aktuelPageService.GetAktuelPagesByAktuelId(aktuel.Id);
            if (aktuelPages != null && aktuelPages.Count() > 0) return Ok(aktuelPages);

            return NotFound();
        }

        // GET api/v1/aktuels/5
        [HttpGet("{id}")]
        public IActionResult Get(int? id)
        {
            if (!id.HasValue) return BadRequest();

            var aktuel = aktuelService.GetAktuel(id.Value);
            if (aktuel == null) return NotFound();

            return Ok(aktuel);
        }

        // GET api/v1/aktuels/search?query=
        [HttpGet("search")]
        public IActionResult search(string query)
        {
            if (string.IsNullOrEmpty(query)) return BadRequest();

            var aktuels = aktuelService.Search(query);
            if (aktuels == null) return NotFound();

            return Ok(aktuels);
        }

        // POST api/v1/aktuels
        [Authorize]
        [HttpPost]
        public IActionResult Post([FromBody]AktuelDto aktuel)
        {
            if (ModelState.IsValid && aktuelService.AddAktuel(aktuel) != null)
            {
                return Ok(aktuel);
            }

            return BadRequest();
        }

        // PUT api/v1/aktuels/5
        [Authorize]
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

        // DELETE api/v1/aktuels/5
        [Authorize]
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

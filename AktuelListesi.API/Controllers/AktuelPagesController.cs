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
    public class AktuelPagesController : ApiController
    {
        private readonly IAktuelPageService aktuelPageService;
        public AktuelPagesController(IAktuelPageService aktuelPageService)
        {  
            this.aktuelPageService = aktuelPageService;
        }

        // GET api/v1/aktuelPages
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(aktuelPageService.GetAktuelPages());
        }
        
        // GET api/v1/aktuelPages/5
        [HttpGet("{id}")]
        public IActionResult Get(int? id)
        {
            if (!id.HasValue) return BadRequest();

            var aktuelPage = aktuelPageService.GetAktuelPage(id.Value);
            if (aktuelPage == null) return NotFound();

            return Ok(aktuelPage);
        }

        // POST api/v1/aktuelPages
        [HttpPost]
        public IActionResult Post([FromBody]AktuelPageDto aktuelPage)
        {
            if (ModelState.IsValid && aktuelPageService.AddAktuelPage(aktuelPage) != null)
            {
                return Ok(aktuelPage);
            }

            return BadRequest();
        }

        // PUT api/v1/aktuelPages/5
        [HttpPut("{id}")]
        public IActionResult Put(int? id, [FromBody]AktuelPageDto aktuelPage)
        {
            if (!id.HasValue) return BadRequest();

            var akt = aktuelPageService.GetAktuelPage(id.Value);
            if (akt == null) return NotFound();
            aktuelPage.Id = akt.Id;
            if (ModelState.IsValid && aktuelPageService.UpdateAktuelPage(aktuelPage) != null)
            {
                return Ok(aktuelPage);
            }

            return BadRequest();
        }

        // DELETE api/v1/aktuelPages/5
        [HttpDelete("{id}/{isSoftDelete}")]
        public IActionResult Delete(int? id, bool isSoftDelete = true)
        {
            if (!id.HasValue) return BadRequest();

            var aktuelPage = aktuelPageService.GetAktuelPage(id.Value);
            if (aktuelPage == null) return NotFound();

            bool isSuccess = (isSoftDelete) ?
                                aktuelPageService.SoftDeleteAktuelPage(id.Value) :
                                aktuelPageService.HardDeleteAktuelPage(id.Value);

            return Ok(isSuccess);
        }
    }
}

using MarketCatalog.Shared.Services;
using Microsoft.AspNetCore.Mvc;

namespace MarketCatalog.API.Controllers
{
    [ApiController]
    [Route("v1/catalogs")]
    public class CatalogsController : ControllerBase
    {
        private readonly ICatalogService _catalogService;
        public CatalogsController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }
    }
}
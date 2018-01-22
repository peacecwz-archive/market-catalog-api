using AktuelListesi.Models.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AktuelListesi.API
{
    public class ApiController : Controller
    {
        public override OkObjectResult Ok(object value)
        {
            var result = new ActionResultModel()
            {
                Data = value,
                IsSuccess = true,
                Messages = null
            };
            return base.Ok(result);
        }
       

        public override NotFoundObjectResult NotFound(object value)
        {

            var result = new ActionResultModel()
            {
                Data = null,
                IsSuccess = false,
                Messages = new List<string>()
                {
                    "No Found any Items"
                }
            };
            return base.NotFound(result);
        }
    }
}

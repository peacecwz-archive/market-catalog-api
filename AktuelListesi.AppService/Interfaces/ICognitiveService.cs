using AktuelListesi.AppService.Models;
using AktuelListesi.Models.AppServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace AktuelListesi.AppService.Interfaces
{
    public interface ICognitiveService
    {
        CognitiveServiceOptions ServiceOptions { get; set; }
        CognitiveServiceModel ReadTextFromImage(string ImageUrl);
    }
}

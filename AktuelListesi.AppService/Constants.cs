using System;
using System.Collections.Generic;
using System.Text;

namespace AktuelListesi
{
    public class Constants
    {
        public static string BaseURL { get; set; } = $"https://aktuellistesi.azurewebsites.net";
        public static string Version { get; set; } = "v1";
        public static string UpdateEndpoint { get; set; } = $"{BaseURL}/api/{Version}/app/update";
        public static string InitializeEndpoint { get; set; } = $"{BaseURL}/api/{Version}/app/initialize";
    }
}

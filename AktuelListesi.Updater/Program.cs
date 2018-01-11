using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AktuelListesi.Updater
{
    class Program
    {
        static void Main(string[] args)
        {
            Log("Started Updater Webjob");
            Run().Wait();
            Log("Ended Updater Webjob");
        }

        static async Task Run()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                Log($"Sending Request to {Constants.UpdateEndpoint}");
                var request = await client.GetAsync(Constants.UpdateEndpoint);
                Log($"Sent Request and HTTP Status Code: {request.StatusCode}");
            }
        }

        static void Log(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}

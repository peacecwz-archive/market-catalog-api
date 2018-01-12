using System;
using AktuelListesi.AppService;
using AktuelListesi.AppService.Interfaces;
using AktuelListesi.Data.Dtos;

namespace AktuelListesi.ImageAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            IQueueService queueService = new QueueService(); 
            var queueItem = queueService.GetNextQueueMessage<AktuelPageDto>();
            if(queueItem!=null)
                Console.WriteLine(queueItem.PageImageUrl);
            else
                Console.WriteLine("Queue Item is null");
        }
    }
}

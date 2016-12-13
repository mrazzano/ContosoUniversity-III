using System;
using System.Web.Configuration;
using Microsoft.Owin.Hosting;

namespace ContosoUniversity.Service
{
    public class Program
    {
        static void Main()
        {
            var baseUri = WebConfigurationManager.AppSettings["ApiUrl"];

            Console.WriteLine("Starting web Server...");
            
            // Start OWIN host 
            using (WebApp.Start<Startup>(url: baseUri))
            {
                Console.WriteLine("Server running at {0} - press Enter to quit. ", baseUri);
                Console.ReadLine();
            }
        }
    }
}
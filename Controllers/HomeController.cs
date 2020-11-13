using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DEMO2.Models;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace DEMO2.Controllers
{
    public class  myfact
    {
        string id; string text; string source;
        string source_url; string language; string permalink;

        public string Id { get => id; set => id = value;}
        public string Text { get => text; set => text  = value;}
        public string Source { get => source; set => source = value;}
        public string SourceUrl { get => source_url; set => source_url = value;}
        public string Language { get => language; set => language = value;}
        public string Permalink { get => permalink; set => permalink = value;}

        public void fact (string i, string t, string s, string u, string l, string p)
        {
            id = i; text = t; source = s;
            source_url = u; language = l; permalink = p;
        }
    }

    public class HomeController : Controller
    {
        myfact f = new myfact();
        static int n = 0;
        
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
          
             ViewData["fact"] = getRandomFact().Text;
             ViewData["count"] = n;
            
            return View();
        }

        public IActionResult Health()
        {
            _logger.LogInformation("I'm Healthy");
            Console.WriteLine("I'm Healthy");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public myfact getRandomFact()
        {

            n++;

            WebRequest request = WebRequest.Create ("https://uselessfacts.jsph.pl/random.json?language=en");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse ();
            Stream dataStream = response.GetResponseStream ();
            StreamReader reader = new StreamReader (dataStream);
        
            f = JsonConvert.DeserializeObject<myfact>(reader.ReadToEnd ());

            _logger.LogInformation(n + "\tStatus = " + response.StatusDescription + " >>> " + f.Text);
            
            // Cleanup the streams and the response.
            reader.Close ();
            dataStream.Close ();
            response.Close ();

            return f;       
        }
    }
}

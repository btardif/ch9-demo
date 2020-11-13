using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ch9_demo.Models;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace ch9_demo.Controllers
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
            ViewData["test"] = getRandomFact().Text;

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

            System.Diagnostics.Trace.TraceInformation(n + "\tStatus = " + response.StatusDescription + " >>> " + f.Text);

            // Cleanup the streams and the response.
            reader.Close ();
            dataStream.Close ();
            response.Close ();

            if(n % 13 == 0) //Unlucky 13
            {
                n=12;

                Console.WriteLine("ERROR >>>>> UNLUCKY 13!");
                                
                throw new WebException ("UNLUCKY 13!", WebExceptionStatus.UnknownError);
            }
            else
            {
                return f;
            }           
        }


    }
}

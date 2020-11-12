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
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            myfact f = new myfact();

            f = getRandomFact();

            Console.WriteLine (f.Text);

            ViewData["test"] = f.Text;

            

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public myfact getRandomFact()
        {
            // Create a request for the URL. 		
            WebRequest request = WebRequest.Create ("https://uselessfacts.jsph.pl/random.json?language=en");
            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;
            // Get the response.
            HttpWebResponse response = (HttpWebResponse)request.GetResponse ();
            // Display the status.
            Console.WriteLine (response.StatusDescription);
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream ();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader (dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd ();
            // Display the content.
            // Console.WriteLine (responseFromServer);
            
            myfact f = new myfact();
            
            f = JsonConvert.DeserializeObject<myfact>(responseFromServer);

            
            
            // Cleanup the streams and the response.
            reader.Close ();
            dataStream.Close ();
            response.Close ();

            return f;
        }


    }
}

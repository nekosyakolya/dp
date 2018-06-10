using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Frontend.Models;
using System.Net.Http;

namespace Frontend.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(string data)
        {

            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("http://localhost:5000"); 
            FormUrlEncodedContent content = new FormUrlEncodedContent(new[] 
            { 
                new KeyValuePair<string, string>("value", data) 
            });

            var result = await client.PostAsync("/api/values", content);
            string id = await result.Content.ReadAsStringAsync();
          
            string newUrl = "http://localhost:5001/Home/TextDetails?=" + id;
            return new RedirectResult(newUrl);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        private async Task<string> GetData(string url)
        {

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);

            string result = response.StatusCode.ToString();
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
            }
            return result;

        }

        public IActionResult TextDetails(string id)
        {
            
            string value = GetData("http://localhost:5000/api/values/"+ id).Result;

            ViewData["Message"] = value;
            return View();
        }
    }
}

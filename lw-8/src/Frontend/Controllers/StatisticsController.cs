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
    public class StatisticsController : Controller
    {
        public IActionResult Index()
        {
            HttpClient client = new HttpClient();
            Task<string> statistics = SendGetRequest("http://localhost:5000/api/values/statistics");
            string[] items = statistics.Result.Split(":");
            ViewData["Message"] = new List<string>()
			{
				"Text Num: " + items[0],
				"High Rank Part: " + items[1],
				"Avg Rank: " + items[2]
			};

            return View();
        }
        private async Task<string> SendGetRequest(string url)
		{
            HttpClient httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            string result = response.StatusCode.ToString();
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
            }
            return result;
        }

    }
}

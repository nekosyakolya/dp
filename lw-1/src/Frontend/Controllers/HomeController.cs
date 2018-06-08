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
            //TODO: send data in POST request to backend and read returned id value from response
            /*
            создаем http cliient
            отправляем через него post запрос на localhost:5000/api/values с данными из параметра
            результат пост запроса содержит id
            */
            return Ok(id);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

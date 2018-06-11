using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Threading;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
     {
        Redis _redis = new Redis();

        RabbitMQ _rabbitMQ = new RabbitMQ();

        [HttpGet("statistics")]
        public IActionResult GetStatistics()
        { 
            string value = _redis.GetStatistics();
            return Ok(value);
        }


        // GET api/values/<id>
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            for (short i = 0; i < 5; ++i)
            {
                string value = _redis.Get("rank:" + id);
                if (value != null)
                {
                    return Ok(value);
                }
                Thread.Sleep(100);
            }
             return new NotFoundResult();
        }

        // POST api/values
        [HttpPost]
        public string Post([FromForm]string value)
        {
            var id = Guid.NewGuid().ToString();
            _redis.Add(new KeyValuePair<string, string>(id, value));
            _rabbitMQ.Add(id);
            return id;
        }

    }
}

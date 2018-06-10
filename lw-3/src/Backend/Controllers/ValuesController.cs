using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;



namespace Backend.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
     {
        Redis _redis = new Redis();

        RabbitMQ _rabbitMQ = new RabbitMQ();
        // GET api/values/<id>
        [HttpGet("{id}")]
        public string Get(string id)
        {
             return _redis.Get(id);
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

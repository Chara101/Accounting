using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AccountAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JustinTestController : ControllerBase
    {
        // GET: api/<JustinTestController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<JustinTestController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "這是你的第一個 Web API 回應！";
        }

        // POST api/<JustinTestController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<JustinTestController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<JustinTestController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

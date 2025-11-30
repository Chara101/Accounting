using Microsoft.AspNetCore.Mvc;

namespace AccountAPI.Controllers
{
        [ApiController]
        [Route("api/[controller]")]
        public class TestController : ControllerBase
        {
            //[HttpGet]
            //public IActionResult GetMessage()
            //{
            //    return Ok("這是你的第一個 Web API 回應！");
            //}

            [HttpGet]
            public IEnumerable<string> Get()
            {
                return new string[] { "value1", "value2" };
            }
        }
}

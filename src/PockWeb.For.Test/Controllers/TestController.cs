using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PockWeb.For.Test.Model;

namespace PockWeb.For.Test.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {

        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Post([FromBody] PostModel model)
        {
            string ticks = System.DateTime.Now.Ticks.ToString();
            _logger.LogInformation("Post [{ticks}]", ticks);
            return Ok(new ResponseModel(true, $"POST OK - {model?.ToString()}"));
        }

        [HttpPost("secret")]
        public IActionResult PostSecret([FromBody] PostModel model)
        {
            _logger.LogInformation("Post Secret");
            return Ok(new ResponseModel(true, $"POST SECRET OK - {model?.ToString()}"));
        }

        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("Get");
            return Ok(new ResponseModel(true, "GET OK"));
        }

        [HttpGet("warning")]
        public IActionResult Warning()
        {
            _logger.LogWarning("Get");
            return Ok(new ResponseModel(true, "WARNING OK"));
        }

        [HttpGet("error")]
        public IActionResult Error()
        {
            _logger.LogWarning("Error");
            throw new System.Exception("ERROR TEST");
        }

        [HttpPost("ignore")]
        public IActionResult PostIgnore([FromBody] PostModel model)
        {
            return Ok(new ResponseModel(true, $"POST IGNORE OK - {model?.ToString()}"));
        }

        [HttpGet("ignore")]
        public IActionResult GetIgnore()
        {
            return Ok(new ResponseModel(true, "GET IGNORE OK"));
        }

    }

}

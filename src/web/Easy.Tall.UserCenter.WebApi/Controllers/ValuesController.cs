using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Easy.Tall.UserCenter.Entity.Model;
using Easy.Tall.UserCenter.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Easy.Tall.UserCenter.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger<ValuesController> _logger;

        private readonly IUserServices _userServices;

        public ValuesController(IUserServices userServices, ILogger<ValuesController> logger)
        {
            _userServices = userServices;
            _logger = logger;
            
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            _userServices.Add(new User());
            _logger.LogDebug("测试一下");
            _logger.LogError("测试一下1");
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

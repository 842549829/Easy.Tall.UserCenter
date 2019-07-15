using System.Collections.Generic;
using CSRedis;
using Easy.Tall.UserCenter.Framework.Attribute;
using Easy.Tall.UserCenter.IServices;
using Easy.Tall.UserCenter.WebApi.Attribute;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Easy.Tall.UserCenter.WebApi.Controllers
{
    /// <summary>
    /// val
    /// </summary>
    [AllowAnonymous]
    [PermissionFilter]
    [Permission("/UserCenter/Values")]
    public class ValuesController : AuthController
    {
        private readonly ILogger<ValuesController> _logger;

        private readonly IUserService _userServices;

        private readonly CSRedisClient _redisClient;

        private readonly IRedisCacheService<CSRedisClient> _redisCacheService;

        private readonly IRoleService _roleService;

        /// <summary>
        /// ValuesController
        /// </summary>
        /// <param name="userServices">userServices</param>
        /// <param name="redisClient">redisClient</param>
        /// <param name="redisCacheService">redisCacheService</param>
        /// <param name="roleService">roleService</param>
        /// <param name="logger">logger</param>
        public ValuesController(IUserService userServices,
            CSRedisClient redisClient,
            IRedisCacheService<CSRedisClient> redisCacheService,
            IRoleService roleService,
            ILogger<ValuesController> logger)
        {
            _userServices = userServices;
            _logger = logger;
            _redisClient = redisClient;
            _redisCacheService = redisCacheService;
            _roleService = roleService;
        }

        /// <summary>
        /// GET api/values
        /// </summary>
        /// <returns>string</returns>
        [HttpGet]
        [Permission("/UserCenter/Values/Get")]
        public ActionResult<IEnumerable<string>> Get()
        {
            var roles = _roleService.GetRoleGroupByResponses();


            var countService = _redisCacheService.GetRedisClient().SAdd("keyService", "xx", "xxx");
            var relService = _redisCacheService.GetRedisClient().SIsMember("keyService", "xx");


            var count = _redisClient.SAdd("key", "xx", "xxx");
            var rel = _redisClient.SIsMember("key", "xx");

            _userServices.Add(new Entity.Extend.UserAddRequest
            {
                Account = "xxx",
                Nickname = "ddd",
                Password = "5544414"
            });

            var d = _userServices.GetPagination(new Entity.Extend.UserFilter { Nickname = "dddxx" });
            _logger.LogDebug("测试一下");
            _logger.LogError("测试一下1");
            var result = new string[] { "value1", "value2" };
            return Ok(result);
        }

        /// <summary>
        /// GET api/values/5
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>string</returns>
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        /// <summary>
        /// POST api/values
        /// </summary>
        /// <param name="value">string</param>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        /// <summary>
        /// PUT api/values/5
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="value">value</param>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        /// <summary>
        /// DELETE api/values/5
        /// </summary>
        /// <param name="id">id</param>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
